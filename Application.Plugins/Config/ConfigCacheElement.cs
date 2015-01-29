using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Plugins.Caching;

namespace Application.Plugins.Config
{

    public sealed class ConfigCacheElement : ConfigurationElement
    {
        [ConfigurationProperty("type", DefaultValue =CachePolicyType.FileWatch,IsRequired=false)]
        public CachePolicyType Type
        {
            get
            {
                return (CachePolicyType)this["type"];
            }
        }

        [ConfigurationProperty("cacheExpiryInterval", DefaultValue = (double)5000, IsRequired = false)]
        public double CacheExpiryInterval
        {
            get
            {
                return (double)this["cacheExpiryInterval"];
            }
        }

        [ConfigurationProperty("slidingExpiration", DefaultValue = true, IsRequired = false)]
        public bool SlidingExpiration
        {
            get
            {
                return (bool)this["slidingExpiration"];
            }
        }


        [ConfigurationProperty("filesystemWatcherDelay", DefaultValue = (double)1000, IsRequired = false)]
        public double FilesystemWatcherDelay
        {
            get
            {
                return (double)this["filesystemWatcherDelay"];
            }
        }

        [ConfigurationProperty("autoReloadOnCacheExpire", DefaultValue = true, IsRequired = false)]
        public bool AutoReloadOnCacheExpire
        {
            get
            {
                return (bool)this["autoReloadOnCacheExpire"];
            }
        }


    }
}
