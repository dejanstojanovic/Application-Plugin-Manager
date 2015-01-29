using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Application.Sample.Base;
using Application.Plugins.Caching;

namespace Application.Plugins.Testing
{
    [TestClass]
    public class PluginManager_Tests : BaseTest
    {
        [TestMethod]
        public void EmplyConstructorTest()
        {
            PluginManager manager = new PluginManager();
        }

        [TestMethod]
        public void TimeIntervalCachingConstructorTest()
        {
            PluginManager manager = new PluginManager(new CachePolicy()
            {
                PolicyType = CachePolicyType.TimeInterval,
                AutoReloadOnCacheExpire = true,
                CacheExpiryInterval = 5000
            });
        }

        [TestMethod]
        public void FilesystemCachingConstructorTest()
        {
            PluginManager manager = new PluginManager(new CachePolicy()
            {
                PolicyType = CachePolicyType.FileWatch,
                FilesystemWatcherDelay = 3000
            });
        }

        [TestMethod]
        public void HybridCachingConstructorTest()
        {
            PluginManager manager = new PluginManager(new CachePolicy()
            {
                PolicyType = CachePolicyType.FileWatch | CachePolicyType.TimeInterval,
                FilesystemWatcherDelay = 1000,
                CacheExpiryInterval = 5000,
                AutoReloadOnCacheExpire = true
            });
        }

        [TestMethod]
        public void PluginLoadTest()
        {
            using (PluginManager manager = new PluginManager())
            {
                var basePlugins = manager.GetPlugin<BasePlugin>("Application.Sample.Plugin1").ToList();
                Assert.AreEqual(basePlugins.Count, 1);

                var nativePlugins = manager.GetPlugin("Application.Sample.Plugin1").ToList();
                Assert.AreEqual(nativePlugins.Count, 2);
            }
        }
    }
}
