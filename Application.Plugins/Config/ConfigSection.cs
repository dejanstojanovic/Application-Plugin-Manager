using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Plugins.Config
{

    public sealed class ConfigSection : ConfigSectionBase
    {
        [ConfigurationProperty("pluginsFolder", DefaultValue = "Plugins", IsRequired = false)]
        public String PluginsFolder
        {
            get
            {
                return this["pluginsFolder"] != null ? this["pluginsFolder"].ToString() : string.Empty;
            }
        }

        [ConfigurationProperty("keepFileHandle", DefaultValue = false, IsRequired = false)]
        public bool KeepFileHandle
        {
            get
            {
                return (bool)this["keepFileHandle"];
            }
        }


        [ConfigurationProperty("createPluginsFolder", DefaultValue = true, IsRequired = false)]
        public bool CreatePluginsFolder
        {
            get
            {
                return (bool)this["createPluginsFolder"];
            }
        }

        [ConfigurationProperty("cache", IsRequired = false)]
        public ConfigCacheElement Cache
        {
            get
            {
                return (ConfigCacheElement)base["cache"];
            }
        }


    }
}
