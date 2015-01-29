using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Plugins
{
    /// <summary>
    /// Event arguments returned in a custom events raised from plugin manager instance
    /// </summary>
    public class PluginEventArgs: EventArgs
    {
        private string assemblyPath;
        /// <summary>
        /// Path of assembly which is raising event
        /// </summary>
        public string AssemblyPath
        {
            get
            {
                return this.assemblyPath;
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="assemblyPath"></param>
        public PluginEventArgs(string assemblyPath)
        {
            this.assemblyPath = assemblyPath;
        }
    }
}
