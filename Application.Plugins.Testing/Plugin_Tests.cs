using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace Application.Plugins.Testing
{
    [TestClass]
    public class Plugin_Tests:BaseTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var plugin = new Derived.TestPlugin();
        }
    }
}
