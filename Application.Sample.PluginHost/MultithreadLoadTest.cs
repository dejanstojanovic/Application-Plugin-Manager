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
    class MultithreadLoadTest
    {
         static PluginManager manager;

        static void Main(string[] args)
        {

            Console.ReadLine();  

            //MULTITHREAD TEST - CACHE
            using (manager = new PluginManager(new CachePolicy() { PolicyType=CachePolicyType.TimeInterval, CacheExpiryInterval=5000}))
            {
                for (int i = 0; i < 1000; i++)
                {
                    new Thread(() =>
                    {
                        while (true)
                        {
                            Plugin plugin;
                            plugin = manager.GetPlugin("Application.Sample.Plugin1").FirstOrDefault();
                            if (plugin != null)
                            {
                                Console.WriteLine(string.Format("Plugin loaded {0}", DateTime.Now.ToString("HH:mm:ss:fff")));
                            }
                        }
                    }).Start();
                }
            }


            Console.ReadLine();          

        }


    }
}
