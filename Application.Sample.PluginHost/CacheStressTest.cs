using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Configuration;
using Application.Plugins;
using System.Threading;
using Application.Plugins.Caching;
using Application.Sample.Base;
namespace Application.Sample.PluginHost
{
    class CacheStressTest
    {
        static PluginManager manager;

        static void Main(string[] args)
        {
            Console.ReadLine();
            DateTime cacheStartTime;
            DateTime cacheEndTime;

            //TEST - CACHE
            using (manager = new PluginManager(new CachePolicy()
            {
                PolicyType = CachePolicyType.TimeInterval,
                CacheExpiryInterval = 5000
            }))
            {
                cacheStartTime = DateTime.Now;
                for (int i = 0; i < 10000; i++)
                {

                    foreach (var plugin in manager.GetPlugin<BasePlugin>("Application.Sample.Plugin1"))
                    {
                        if (plugin != null)
                        {
                            Console.WriteLine(string.Format("Plugin loaded {0}", DateTime.Now.ToString("HH:mm:ss:fff")));
                        }
                    }
                }
                cacheEndTime = DateTime.Now;
            }
            Console.Clear();
            Console.WriteLine("CACHE TEST END");
            Console.Write((cacheEndTime - cacheStartTime).TotalMilliseconds.ToString());
            Console.ReadLine();
        }


    }
}
