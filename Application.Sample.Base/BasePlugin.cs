using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Sample.Base
{
    public abstract class BasePlugin : Application.Plugins.Plugin, IBasePlugin
    {

        public BasePlugin(string path)
            : base(path)
        {

        }

        public ConsoleColor ColorOfConsole { get; set; }
        public abstract void DoSomething(params string[] parameters);

        /// <summary>
        /// Dummy method forded implementation by IBasePlugin
        /// </summary>
        public void SomeDummyMethod()
        {
            Console.WriteLine("Executing method SomeDummyMethod");
            Thread.Sleep(1500);
        }
    }
}
