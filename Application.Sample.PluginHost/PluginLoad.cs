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
    class PluginLoad
    {
         static PluginManager manager;

        static void Main(string[] args)
        {
            Console.WriteLine("Hit enter when ready");
            Console.ReadLine();
            //Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff"));

            CachePolicy policy = CachePolicy.GetTimeIntervalCachePolicy();
            policy.AutoReloadOnCacheExpire = true;
            policy.CacheExpiryInterval = 2000;
            policy.SlidingExpiration = true;
            //PluginManager manager;

            manager = new PluginManager(policy);

            var a = manager.GetPlugin<BasePlugin>("Application.Sample.Plugin1").ToList();

            //foreach (var plugin in manager.GetPlugin<BasePlugin>("Application.Sample.Plugin1"))
            //{
            //    //do something with plugin
            //}

            manager.AssemblyRemovedFromCache += manager_AssemblyRemovedFromCache;
            manager.AssemblyLoaded += manager_AssemblyLoaded;

            
            Console.ReadLine();
        }

        static void manager_AssemblyLoaded(object sender, PluginEventArgs e)
        {
            Console.WriteLine(string.Format("{0} {1}", e.AssemblyPath, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")));
        }

        static void manager_AssemblyRemovedFromCache(object sender, PluginEventArgs e)
        {
            
        }

    }
}
