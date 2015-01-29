using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Plugins
{
    /// <summary>
    /// Plugin base class
    /// </summary>
    public abstract class Plugin : IPlugin
    {
        #region Implementation code

        #region Fields
        private string assemblyLocation;
        private System.Configuration.Configuration pluginConfiguration;
        private System.Configuration.Configuration hostConfiguration;
        #endregion

        #region Properties
        /// <summary>
        /// Loaded assembly file path
        /// </summary>
        public string AssemblyLocation
        {
            get
            {
                return assemblyLocation;
            }
        }

        /// <summary>
        /// Plugin related cnfiguration
        /// </summary>
        public System.Configuration.Configuration PluginConfiguration
        {
            get
            {
                return pluginConfiguration;
            }
        }

        /// <summary>
        /// Configuration of hosting aplication
        /// </summary>
        public System.Configuration.Configuration HostConfiguration
        {
            get
            {
                return hostConfiguration;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default parameterless constructor
        /// </summary>
        protected Plugin()
        {
            hostConfiguration = ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().Location);
            this.pluginConfiguration = hostConfiguration;
        }

        /// <summary>
        /// Default cnstructor
        /// </summary>
        /// <param name="path">File path of loaded assembly to load it's own configuration</param>
        protected Plugin(string path = null)
        {
            assemblyLocation = path;
            if (!string.IsNullOrWhiteSpace(assemblyLocation) && File.Exists(string.Format("{0}.config", AssemblyLocation)))
            {
                this.pluginConfiguration = ConfigurationManager.OpenExeConfiguration(AssemblyLocation);
            }
            else
            {
                this.pluginConfiguration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
            }

            hostConfiguration = ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().Location);
        }
        #endregion

        #endregion




    }
}
