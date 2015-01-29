using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Plugins
{
    /// <summary>
    /// Basic plugin interface
    /// </summary>
    public interface IPlugin
    {

        #region Implementation code
        /// <summary>
        /// File path of the loaded assembly
        /// </summary>
        string AssemblyLocation
        {
            get;
        }

        /// <summary>
        /// Plugin related configuration
        /// </summary>
        System.Configuration.Configuration PluginConfiguration
        {
            get;
        }

        /// <summary>
        /// Plugin host configuration
        /// </summary>
        System.Configuration.Configuration HostConfiguration
        {
            get;
        }

        #endregion


    }
}
