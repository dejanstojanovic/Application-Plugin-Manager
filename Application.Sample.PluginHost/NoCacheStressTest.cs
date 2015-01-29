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
    class NoCacheStressTest
    {
        static PluginManager manager;


        static void Main(string[] args)
        {
            //Console.ReadLine();
            DateTime noCacheStartTime;
            DateTime noCacheEndTime;

            //TEST - NO CACHE
            using (manager = new PluginManager(CachePolicy: null))
            {
                noCacheStartTime = DateTime.Now;
                for (int i = 0; i < 10000; i++)
                {

                    foreach (var plugin in manager.GetPlugin<BasePlugin>("Application.Sample.Plugin1"))
                    {
                        if (plugin != null)
                        {
                            //Console.WriteLine(string.Format("Plugin loaded {0}", DateTime.Now.ToString("HH:mm:ss:fff")));
                        }
                    }
                }
                noCacheEndTime = DateTime.Now;
            }
            Console.Clear();
            Console.WriteLine("NO CACHE TEST END");
            Console.Write((noCacheEndTime - noCacheStartTime).TotalMilliseconds.ToString());
            //Console.ReadLine();
        }



    }
}
