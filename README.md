# Application-Plugin-Manager
Advanced plugin management in application, especially deigned for heavy load services which requires high performance in multi-thread environment while keeping hight level of extensibility with 0% downtime when upgrading the service functionality.

###Performance boost with using plugin assembly caching

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
