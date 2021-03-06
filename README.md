# Application-Plugin-Manager **[DEPRICATED]**

-------------------------------------------------------------------------------------------------------------------------
###_This project is depricated since I made it too complicated over time_ :sunglasses: _. Will come up with something more elegant soon!_
###_Stay tuned..._
-------------------------------------------------------------------------------------------------------------------------
Advanced plugin management in application, especially deigned for heavy load services which requires high performance in multi-thread environment while keeping hight level of extensibility with 0% downtime when upgrading the service functionality.

###What is it
The initial idea for this library was to use simple plugin approach described in my blog [http://dejanstojanovic.net/aspnet/2014/october/simple-plugin-host-application-approach/](http://dejanstojanovic.net/aspnet/2014/october/simple-plugin-host-application-approach/). 

This simple approach can be useful for desktop application when your plugin will be loadad in your app most probably only once, but in case you want to use this approach in some very busy services, you might get out of the memory pretty quickly. That is why I started working on this library to enable using plugins without worries that application will loose on performance even though functinality is splited into seperate assembly files.

_This is not replacement for MEF and it is not intended to be. It is just a bit different approach trying to solve some issues which MEF(Managed Extensibility Framework) does not solve by default_
ME
###Differences comparing to MEF
This solution solves few problems which MEF doesn't:
- Assembly file locking
- Reloading assemblies on the runtime with simple file replacement
- No need for host application restart when plugin assembly is updated
- Allows assembly autoreload with caching rules

###Differences comparing to MAF
MAF uses separate app domains for code isolation which is based on object serialization. Using object serialization affects performances.


####Assembly file locking
When files are loaded by PluginManager they can be released alloving them to be replaced with new assemblies. For example if you fix some bugs in your plugin, you can just got to your application plugins folder and drop your assembly. It will be automatically picked up by your host application

####Reloading assemblies on the runtime with simple file replacement
As mentioned in the prevous point, just upload new assembly to host application plugins foder and it will be re-loaded according to PluginManager CachingPolicy

####No need for host application restart when plugin assembly is updated
If your host application is pretty busy Windows Service which needs to mantian 100% uptime, you should try this library because it does not require host application stopping to include new functionality.
even if you add new plugin which needs to be loaded by a specific rule, just uplaod it to host application plugins folder and it will be availabe in the host application.

####Allows assembly autoreload with caching rules
Depending on caching rules, ApplicationManager can cache instances of assemblies that inherit Plugin class and mantain low memory usage. Assemblies can be reloaded when assembly is changes, when certain period of time expires or both combined.
Also if assembly is used very often than it can be kept or released by using sliding time expiration setting in cache policy of PluginManager instance.

###Performance boost with using plugin assembly caching
The real power of application manager can be noticed when used ih multitherad, frequent plugin assembly load such as WCF services or similar services which need to process large amount of request using distributed code in isolated assemblies.

####High load of plugins without caching used
```cs
static void Main(string[] args)
{
    DateTime noCacheStartTime;
    DateTime noCacheEndTime;
    using (PluginManager manager = new PluginManager(CachePolicy: null))
    {
		noCacheStartTime = DateTime.Now;
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
		noCacheEndTime = DateTime.Now;
    }
    Console.Clear();
    Console.WriteLine("NO CACHE TEST END");
    Console.Write((noCacheEndTime - noCacheStartTime).TotalMilliseconds.ToString());
}
```

####High load of plugins without caching used
```cs
static void Main(string[] args)
{
    DateTime cacheStartTime;
    DateTime cacheEndTime;
    using (PluginManager manager = new PluginManager(new CachePolicy()
    {
		PolicyType = CachePolicyType.TimeInterval,
		CacheExpiryInterval = 50000
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
}
```
###Known issues
Since assemblies are dynamically loaded during runtime at the same application domain, to avoid performance downgrade, it is not possible to unload previous vresion of assembly while the host is running.
It reflects by occupying mamanged memory, but not significantly.
Practically, plugin assemblies are not updated frequently which does not cause significant memory leak
