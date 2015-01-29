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
namespace Application.Sample.PluginHost
{
    class PluginLoad
    {
         static PluginManager manager;

        static void Main(string[] args)
        {
            Console.WriteLine("Hit enter when ready");
            Console.ReadLine();
            //Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff"));

            CachePolicy policy = CachePolicy.GetTimeIntervalCachePolicy();
            policy.AutoReloadOnCacheExpire = false;
            policy.CacheExpiryInterval = 5000;
            policy.SlidingExpiration = true;
            //PluginManager manager;

            manager = new PluginManager(policy);

            manager.AssemblyRemovedFromCache += manager_AssemblyRemovedFromCache;
            manager.AssemblyLoaded += manager_AssemblyLoaded;

            var plugins = manager.GetPlugin<Plugin>("Application.Sample.Plugin1").ToList();
            new Thread(() => {
                while (true) {
                    manager.GetPlugin<Plugin>("Application.Sample.Plugin1").ToList();
                    Thread.Sleep(1000);
                }
            }).Start();

            Console.ReadLine();
        }

        static void manager_AssemblyLoaded(object sender, PluginEventArgs e)
        {
            
        }

        static void manager_AssemblyRemovedFromCache(object sender, PluginEventArgs e)
        {
            
        }

    }
}
