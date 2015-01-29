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
    class Program
    {

        static void Main(string[] args)
        {

            var manager = new PluginManager(new CachePolicy()
            {
                PolicyType = CachePolicyType.TimeInterval,
                CacheExpiryInterval = 3000
            }
            );
            manager.AssemblyLoaded += factory_AssemblyLoaded;

            //MULTITHREAD TEST
            /*
            for (int i = 0; i < 10; i++)
            {
                new Thread(() =>
                {
                    while (true)
                    {
                        ApplicationPlugins.IPlugin plugin;
                        plugin = factory.GetPlugin("Plugin1");
                        if (plugin != null)
                        {
                            plugin.DoSomeWork();
                            Console.WriteLine(string.Format("Plugin loaded {0}", DateTime.Now.ToString("HH:mm:ss:fff")));
                        }
                    }
                }).Start();
            }
            */

            //LOADING TEST

            foreach (var plugin in manager.GetPlugin<Plugin>("Plugin1"))
            {


            }

            //Application.Plugins.IPlugin plugin;
            //plugin = manager.GetPlugin("Plugin1").First();
            //Console.WriteLine("Update dll and hit enter");
            Console.ReadLine();

            //plugin = manager.GetPlugin("Plugin1").First();
            //Console.WriteLine("Press any key to end");
            //Console.ReadLine();

            //CONFIG TEST
            /*
            //Config value of the HOST
            ApplicationPlugins.IPlugin plugin;
            Console.WriteLine(ConfigurationManager.AppSettings["appname"]);

            //Config value of the PLUGIN
            plugin = ApplicationPlugins.Manager.GetPluginInstance("Plugin1");
            plugin.DoSomeWork();

            Console.ReadLine();
            */
        }

        static void factory_AssemblyLoaded(Object sender, PluginEventArgs e)
        {
            Console.WriteLine("ASSEBLY LOADED:");
            Console.WriteLine(string.Join(string.Empty, e.AssemblyPath));
        }

    }
}
