using Application.Plugins.Config;
using Application.Plugins.Exceptions;
using Application.Plugins.Resources;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Application.Plugins.Caching;

namespace Application.Plugins
{

    public class PluginManager : IDisposable
    {
        #region Fields
        private Lazy<ConcurrentDictionary<string, PluginAssembly>> assemblies = new Lazy<ConcurrentDictionary<string, PluginAssembly>>(() => new ConcurrentDictionary<string, PluginAssembly>());
        private FileSystemWatcher watcher = null;
        private System.Timers.Timer cacheExpiryTimer = null;
        private string pluginsFolder;
        private bool keepFileHandle;
        private CachePolicy cachePolicy;
        private bool createPluginsFolder;
        #endregion

        #region Properties

        public bool CreatePluginsFolder
        {
            get
            {
                return this.createPluginsFolder;
            }
        }

        /// <summary>
        /// Path of the folder wher plugin assemblies are stored
        /// </summary>
        public string PluginsFolder
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.pluginsFolder))
                {
                    if (Directory.Exists(this.pluginsFolder))
                    {
                        return new DirectoryInfo(this.pluginsFolder).FullName;
                    }
                    else if (HttpContext.Current != null && Directory.Exists(HttpContext.Current.Server.MapPath(this.pluginsFolder)))
                    {
                        return HttpContext.Current.Server.MapPath(this.pluginsFolder);
                    }
                    else if (Directory.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), this.pluginsFolder)))
                    {
                        return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), this.pluginsFolder);
                    }
                    else
                    {
                        return this.pluginsFolder;
                    }
                }
                else
                {
                    return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Plugins");
                }
            }
        }

        /// <summary>
        /// An instance of cache settings
        /// </summary>
        public CachePolicy CachePolicy
        {
            get
            {
                return cachePolicy;
            }
        }

        /// <summary>
        /// Keep handle on the assembly file from which plugin is loaded
        /// </summary>
        public bool KeepFileHandle
        {
            get
            {
                return keepFileHandle;
            }
        }
        #endregion

        #region Events
        public delegate void PluginEventHandler(Object sender, PluginEventArgs e);

        public event PluginEventHandler AssemblyLoaded;
        public event PluginEventHandler AssemblyRemovedFromCache;
        #endregion

        #region Constructors

        /// <summary>
        /// Default parameterless constructor
        /// </summary>
        public PluginManager()
        {
            this.LoadConfiguration();
            this.InitializePluginManager();
        }

        /// <summary>
        /// Intializes class and setting plugins folder
        /// </summary>
        /// <param name="PluginsFolder"></param>
        public PluginManager(string PluginsFolder)
        {
            this.LoadConfiguration();
            this.pluginsFolder = PluginsFolder;

            this.InitializePluginManager();
        }

        /// <summary>
        /// Initializes class and setting caching behaviour
        /// </summary>
        /// <param name="CachePolicy"></param>
        public PluginManager(CachePolicy CachePolicy)
        {
            this.LoadConfiguration();
            this.cachePolicy = CachePolicy;

            this.InitializePluginManager();
        }

        /// <summary>
        /// Initializes the class with value predefined values
        /// </summary>
        /// <param name="PluginsFolder"></param>
        /// <param name="CreatePluginsFolder"></param>
        /// <param name="KeepFileHandle"></param>
        public PluginManager(string PluginsFolder, bool CreatePluginsFolder, bool KeepFileHandle)
        {
            this.LoadConfiguration();
            this.pluginsFolder = PluginsFolder;
            this.createPluginsFolder = CreatePluginsFolder;
            this.keepFileHandle = KeepFileHandle;
            this.InitializePluginManager();
        }

        /// <summary>
        /// Initializes the class with value predefined values
        /// </summary>
        /// <param name="PluginsFolder"></param>
        /// <param name="CachePolicy"></param>
        /// <param name="CreatePluginsFolder"></param>
        /// <param name="KeepFileHandle"></param>
        public PluginManager(string PluginsFolder, CachePolicy CachePolicy, bool CreatePluginsFolder, bool KeepFileHandle)
        {
            this.pluginsFolder = PluginsFolder;
            this.createPluginsFolder = CreatePluginsFolder;
            this.keepFileHandle = KeepFileHandle;
            this.cachePolicy = CachePolicy;

            this.InitializePluginManager();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads manager properties from config file
        /// </summary>
        private void LoadConfiguration()
        {
            var configuration = ConfigurationManager.GetSection("pluginConfigSection") as ConfigSection;
            if (configuration != null)
            {
                this.pluginsFolder = configuration.PluginsFolder;
                this.keepFileHandle = configuration.KeepFileHandle;
                this.createPluginsFolder = configuration.CreatePluginsFolder;
                if (configuration.Cache != null)
                {
                    this.cachePolicy = new CachePolicy()
                    {
                        PolicyType = configuration.Cache.Type,
                        FilesystemWatcherDelay = configuration.Cache.FilesystemWatcherDelay,
                        AutoReloadOnCacheExpire = configuration.Cache.AutoReloadOnCacheExpire,
                        CacheExpiryInterval = configuration.Cache.CacheExpiryInterval,
                        SlidingExpiration = configuration.Cache.SlidingExpiration
                    };
                }
                else
                {
                    this.cachePolicy = new CachePolicy();
                }
            }
        }

        /// <summary>
        /// Initializes components of the manager class based on the properties values
        /// </summary>
        private void InitializePluginManager()
        {
            if (!Directory.Exists(this.PluginsFolder) && this.CreatePluginsFolder)
            {
                Directory.CreateDirectory(this.PluginsFolder);
            }

            if (this.CachePolicy != null && this.CachePolicy.PolicyType.HasFlag(CachePolicyType.TimeInterval))
            {

                this.cacheExpiryTimer = new System.Timers.Timer()
                {
                    Interval = 100,
                    Enabled = true,
                    AutoReset = true
                };

                cacheExpiryTimer.Elapsed += cacheExpiryTimer_Elapsed;
            }

            if (this.CachePolicy != null && this.CachePolicy.PolicyType.HasFlag(CachePolicyType.FileWatch))
            {
                if (Directory.Exists(this.PluginsFolder))
                {
                    watcher = new FileSystemWatcher(this.PluginsFolder, "*.dll")
                    {
                        EnableRaisingEvents = true,
                        IncludeSubdirectories = true
                    };
                    watcher.Changed += watcher_Changed;
                    watcher.Deleted += watcher_Deleted;
                }
                else
                {
                    throw new PluginDirectoryNotFoundException(string.Format(Resources.Strings.PluginDirectoryNotFoundException, this.PluginsFolder));
                }
            }
        }

        /// <summary>
        /// Retrieving all class instances from assembly which are imlementing IPlugin interface and inheriting T 
        /// (type T must implement IPlugin in order to have its type retrieved)
        /// </summary>
        /// <typeparam name="T">Type of plugin base implementing IPlugin interface</typeparam>
        /// <param name="PluginName">Plugin assembly filename or full path to plugin assembly</param>
        /// /// <param name="Subfolder">Optional subfolder name where plugin assembly is located inside plugins folder</param>
        /// <returns></returns>
        public IEnumerable<T> GetPlugin<T>(string PluginName, string Subfolder = null) where T : Plugin, IPlugin
        {
            string pluginPath = string.Empty;
            PluginAssembly loadedAssembly = null;
            IEnumerable<Type> pluginTypes = null;

            if (File.Exists(PluginName))
            {
                pluginPath = new DirectoryInfo(PluginName).FullName.ToLower();
                if (pluginPath.Contains(this.PluginsFolder))
                {
                    throw new PluginFileNotInPluginsFolderException(string.Format(Strings.PluginFileNotInPluginsFolderException, pluginPath, this.PluginsFolder));
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Subfolder))
                {
                    pluginPath = Path.Combine(this.PluginsFolder, string.Format("{0}{1}", PluginName, !PluginName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) ? ".dll" : string.Empty)).Trim().ToLower().ToString();
                }
                else
                {
                    pluginPath = Path.Combine(this.PluginsFolder, Subfolder, string.Format("{0}{1}", PluginName, !PluginName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase) ? ".dll" : string.Empty)).Trim().ToLower().ToString();
                }
            }

            if (!assemblies.Value.ContainsKey(pluginPath))
            {
                loadedAssembly = new PluginAssembly(DateTime.Now, LoadAssemblyFromFilesystem(pluginPath));
                pluginTypes = GetPluginTypes<T>(loadedAssembly.Assembly);
                //Do not add to cache if cache policy not defined or assembly does not have IPlugin classes
                if (this.CachePolicy != null && pluginTypes != null && pluginTypes.Any())
                {
                    //assemblies.Value.TryAdd(pluginPath, loadedAssembly);
                    assemblies.Value.AddOrUpdate(pluginPath, loadedAssembly, (key, oldValue) => loadedAssembly);
                    if (AssemblyLoaded != null)
                    {
                        AssemblyLoaded(this, new PluginEventArgs(pluginPath));
                    }
                }
            }
            else
            {
                //Update load time if sliding expiration
                if (this.CachePolicy.SlidingExpiration)
                {
                    //assemblies.Value.TryGetValue(pluginPath, out loadedAssembly);
                    //assemblies.Value.TryUpdate(pluginPath, new PluginAssembly(DateTime.Now, loadedAssembly.Assembly), loadedAssembly);
                    assemblies.Value.AddOrUpdate(pluginPath, new PluginAssembly(DateTime.Now, loadedAssembly.Assembly), (key, oldValue) => new PluginAssembly(DateTime.Now, loadedAssembly.Assembly));
                }
                else
                {
                    assemblies.Value.TryGetValue(pluginPath, out loadedAssembly);
                }
            }

            if (pluginTypes == null)
            {
                pluginTypes = GetPluginTypes<T>(loadedAssembly.Assembly);
            }

            foreach (var type in pluginTypes)
            {
                var ctor = type.GetConstructor(new Type[] { typeof(string) });
                var plugin = ctor.Invoke(new object[] { pluginPath });
                yield return plugin as T;
            }
        }

        #region Method GetPlugin overloads
        /// <summary>
        /// Retrieve plugin instance as base plugin class 
        /// </summary>
        /// <param name="PluginName">Plugin assembly name of assembly file path</param>
        /// <param name="Subfolder">Optional subfolder name where plugin assembly is located inside plugins folder</param>
        /// <returns></returns>
        public IEnumerable<Plugin> GetPlugin(string PluginName, string Subfolder = null)
        {
            return GetPlugin<Plugin>(PluginName, Subfolder);
        }

        /// <summary>
        /// Retrieve plugin instance as base plugin class 
        /// </summary>
        /// <param name="PluginFileInfo"></param>
        /// <returns></returns>
        public IEnumerable<Plugin> GetPlugin(FileInfo PluginFileInfo)
        {
            return GetPlugin<Plugin>(PluginFileInfo.FullName.ToLower());
        }

        /// <summary>
        /// Retrieving all class instances from assembly which are imlementing IPlugin interface and inheriting T 
        /// (type T must implement IPlugin in order to have its type retrieved)
        /// </summary>
        /// <typeparam name="T">Type of plugin base implementing IPlugin interface</typeparam>
        /// <param name="PluginFileInfo">Plugin assembkly FileInfo instance</param>
        /// <returns></returns>
        public IEnumerable<T> GetPlugin<T>(FileInfo PluginFileInfo) where T : Plugin, IPlugin
        {
            return this.GetPlugin<T>(PluginFileInfo.FullName.ToLower());
        }
        #endregion

        /// <summary>
        /// Retrieves list of types implementing IPlugin interface
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private IEnumerable<Type> GetPluginTypes<T>(Assembly assembly)
        {
            return assembly.GetExportedTypes().Where(t => t.IsClass && typeof(IPlugin).IsAssignableFrom(t) && typeof(T).IsAssignableFrom(t));
        }

        /// <summary>
        /// Loades assembly instance from FileSystem
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private Assembly LoadAssemblyFromFilesystem(string filePath)
        {
            if (File.Exists(filePath))
            {
                if (!KeepFileHandle)
                {
                    return Assembly.Load(File.ReadAllBytes(filePath));
                }
                else
                {
                    return Assembly.LoadFile(filePath);
                }
            }
            else
            {
                throw new PluginFileNotFoundException(string.Format(Strings.PluginFileNotFoundException, filePath));
            }
        }

        #endregion

        #region Event handlers

        void cacheExpiryTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (assemblies != null && assemblies.Value != null)
            {
                var expiredAssemblies = this.assemblies.Value.Where(a => a.Value.LoadTime.AddMilliseconds(this.CachePolicy.CacheExpiryInterval) <= DateTime.Now).ToList();
                if (expiredAssemblies.Any())
                {
                    foreach (var expiredAssembly in expiredAssemblies)
                    {
                        if (this.CachePolicy.AutoReloadOnCacheExpire)
                        {
                            //assemblies.Value.TryUpdate(expiredAssembly.Key, new PluginAssembly(DateTime.Now, this.LoadAssemblyFromFilesystem(expiredAssembly.Key)), expiredAssembly.Value);
                            assemblies.Value.AddOrUpdate(expiredAssembly.Key, 
                                new PluginAssembly(DateTime.Now, this.LoadAssemblyFromFilesystem(expiredAssembly.Key)),
                                (key, oldValue) => new PluginAssembly(DateTime.Now, this.LoadAssemblyFromFilesystem(expiredAssembly.Key)));
                            if (AssemblyLoaded != null)
                            {
                                AssemblyLoaded(this, new PluginEventArgs(expiredAssembly.Key));
                            }
                        }
                        else
                        {
                            PluginAssembly removedAssembly;
                            assemblies.Value.TryRemove(expiredAssembly.Key, out removedAssembly);
                            if (AssemblyRemovedFromCache != null)
                            {
                                AssemblyRemovedFromCache(this, new PluginEventArgs(expiredAssembly.Key));
                            }
                        }
                    }
                }
            }
        }

        protected void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            PluginAssembly loadedAssembly = null;
            PluginAssembly cachedAssembly = null;
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                //Update in cache
                if (assemblies.Value.ContainsKey(e.FullPath.Trim().ToLower()))
                {
                    assemblies.Value.TryGetValue(e.FullPath.Trim().ToLower(), out cachedAssembly);

                    // Realod assembly if file is older than CachePolicy.FilesystemWatcherDelay miliseconds 
                    // to avoid issue with FileSystemWatcher multiple eventson file cache
                    if (cachedAssembly.LoadTime.AddMilliseconds(this.CachePolicy.FilesystemWatcherDelay) < DateTime.Now)
                    {

                        Assembly loadedPlugin = LoadAssemblyFromFilesystem(e.FullPath.Trim().ToLower());
                        if (loadedPlugin != null)
                        {
                            loadedAssembly = new PluginAssembly(DateTime.Now, loadedPlugin);
                            //assemblies.Value.TryUpdate(e.FullPath.Trim().ToLower(), loadedAssembly, cachedAssembly);
                            assemblies.Value.AddOrUpdate(
                                e.FullPath.Trim().ToLower(),
                                loadedAssembly,
                                (key, oldValue) => loadedAssembly);
                            if (AssemblyLoaded != null)
                            {
                                AssemblyLoaded(this, new PluginEventArgs(e.FullPath.Trim().ToLower()));
                            }
                        }
                    }
                }
            }

        }

        protected void watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            PluginAssembly cachedAssembly = null;
            if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                //Remove from cache
                if (assemblies.Value.ContainsKey(e.FullPath.Trim().ToLower()))
                {
                    assemblies.Value.TryRemove(e.FullPath.Trim().ToLower(), out cachedAssembly);
                    if (AssemblyRemovedFromCache != null)
                    {
                        AssemblyRemovedFromCache(this, new PluginEventArgs(e.FullPath.Trim().ToLower()));
                    }
                }
            }
        }
        #endregion

        #region IDisposable

        /// <summary>
        /// Disposes all elements an relases the resources
        /// </summary>
        public void Dispose()
        {
            if (this.cacheExpiryTimer != null)
            {
                this.cacheExpiryTimer.Stop();
                this.cacheExpiryTimer.Enabled = false;
                cacheExpiryTimer.Dispose();
            }
            if (this.watcher != null)
            {
                this.watcher.EnableRaisingEvents = false;
                this.watcher.Dispose();
            }
        }
        #endregion
    }
}