
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Plugins
{
    /// <summary>
    /// Provides base class with singletone PluginManager instance
    /// </summary>
    public abstract class PluginHost
    {
        private static PluginManager pluginManager;

        /// <summary>
        /// Singletone PluginManager instance
        /// </summary>
        public static PluginManager PluginManager
        {
            get
            {
                if (PluginManager != null)
                {
                    pluginManager = new PluginManager();
                }
                return pluginManager;
            }
        }
    }
}
