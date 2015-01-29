using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Plugins.Caching
{

    /// <summary>
    /// Plugin cache configuraton class
    /// </summary>
    public class CachePolicy
    {
        #region Properties
        /// <summary>
        /// Plugin cache type value
        /// </summary>
        public CachePolicyType PolicyType { get; set; }

        /// <summary>
        /// Interval after which plugin will be removed from cache or reloaded (depending on AutoReloadOnCacheExpire value) if type of cache is TimeInterval
        /// </summary>
        public double CacheExpiryInterval { get; set; }

        /// <summary>
        /// Resets assembly load time every time assembly is accessed by GetPlugin method
        /// </summary>
        public bool SlidingExpiration { get; set; }

        /// <summary>
        /// Time interval to start listening for changes on the same file (resolves multiple chnaged even raising issue of FileSystemWatcher)
        /// </summary>
        public double FilesystemWatcherDelay { get; set; }

        /// <summary>
        /// Determines whether assembly will be loaded to cache ater expiration interval elapses or just removed
        /// </summary>
        public bool AutoReloadOnCacheExpire { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public CachePolicy()
        {
            this.PolicyType = CachePolicyType.FileWatch;
            this.FilesystemWatcherDelay = 1000;
            this.AutoReloadOnCacheExpire = true;
            this.CacheExpiryInterval = 5000;
            this.SlidingExpiration = false;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates PluginCachePolicy instance which is based on time interval expiration
        /// </summary>
        /// <param name="AutoReloadOnCacheExpire"></param>
        /// <param name="CacheExpiryInterval"></param>
        /// <returns></returns>
        public static CachePolicy GetTimeIntervalCachePolicy(bool AutoReloadOnCacheExpire = true, double CacheExpiryInterval = 5000, bool SlidingExpiration = false)
        {
            return new CachePolicy()
            {
                PolicyType = CachePolicyType.TimeInterval,
                AutoReloadOnCacheExpire = AutoReloadOnCacheExpire,
                CacheExpiryInterval = CacheExpiryInterval,
                SlidingExpiration = SlidingExpiration
            };
        }

        /// <summary>
        /// /// Creates PluginCachePolicy instance which is based on plugin assembly file change
        /// </summary>
        /// <param name="FilesystemWatcherDelay"></param>
        /// <returns></returns>
        public static CachePolicy GetFileWatchCachePolicy(double FilesystemWatcherDelay = 1000)
        {
            return new CachePolicy()
            {
                PolicyType = CachePolicyType.FileWatch,
                FilesystemWatcherDelay = FilesystemWatcherDelay
            };
        }

        /// <summary>
        /// /// Creates PluginCachePolicy instance which is based on time interval expiration and plugin assembly file change
        /// </summary>
        /// <param name="AutoReloadOnCacheExpire"></param>
        /// <param name="CacheExpiryInterval"></param>
        /// <param name="FilesystemWatcherDelay"></param>
        /// <returns></returns>
        public static CachePolicy GetHybridCachePolicy(bool AutoReloadOnCacheExpire = true, double CacheExpiryInterval = 5000,bool SlidingExpiration=false, double FilesystemWatcherDelay = 1000)
        {
            return new CachePolicy()
            {
                PolicyType = CachePolicyType.TimeInterval | CachePolicyType.FileWatch,
                AutoReloadOnCacheExpire = AutoReloadOnCacheExpire,
                CacheExpiryInterval = CacheExpiryInterval,
                SlidingExpiration = SlidingExpiration,
                FilesystemWatcherDelay = FilesystemWatcherDelay
            };

        }
        #endregion

    }
}
