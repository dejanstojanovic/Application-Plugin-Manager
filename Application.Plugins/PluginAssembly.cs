using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Plugins
{
    internal class PluginAssembly
    {

        #region Fields

        private DateTime loadTime;
        private Assembly assembly;

        #endregion

        #region Properties
        /// <summary>
        /// Date and time when assembly was loaded
        /// </summary>
        public DateTime LoadTime
        {
            get
            {
                return this.loadTime;
            }
        }

        /// <summary>
        /// Loaded assembly instance
        /// </summary>
        public Assembly Assembly
        {
            get
            {
                return this.assembly;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="loadTime">Date and time of assembly loading</param>
        /// <param name="assembly">Assembly instance</param>
        public PluginAssembly(DateTime loadTime, Assembly assembly)
        {
            this.assembly = assembly;
            this.loadTime = loadTime;
        }

        #endregion

    }

}
