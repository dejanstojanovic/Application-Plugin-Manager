# Application-Plugin-Manager
Advanced plugin management in application, especially deigned for heavy load services which requires high performance in multi-thread environment while keeping hight level of extensibility with 0% downtime when upgrading the service functionality.

_Core of this library was developed without using MEF. Future version will heavily relly on Managed Extensibility Framework - MEF (documentation and sample code available on [codeplex](https://mef.codeplex.com/) and  [msdn](https://msdn.microsoft.com/en-us/library/dd460648(v=vs.110).aspx))_

###What is it
The initial idea for this library was to use simple plugin approach described in my blog [http://dejanstojanovic.net/aspnet/2014/october/simple-plugin-host-application-approach/](http://dejanstojanovic.net/aspnet/2014/october/simple-plugin-host-application-approach/). 

This simple approach can be useful for desktop application when your plugin will be loadad in your app most probably only once, but in case you want to use this approach in some very busy services, you might get out of the memory pretty quickly. That is why I started working on this library to enable using plugins without worries that application will loose on performance even though functinality is splited into seperate assembly files.

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
![ScreenShot](http://dejanstojanovic.net/media/31627/no-cache.png)

Managed memory performance graph WITHOUT using advanced plugin assembly caching

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
![ScreenShot](http://dejanstojanovic.net/media/31626/cache.png)

Managed memory performance graph WITH using advanced plugin assembly caching
