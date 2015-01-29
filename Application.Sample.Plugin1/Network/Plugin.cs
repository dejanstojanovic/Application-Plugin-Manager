using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Sample.Plugin1.Network
{
    public class Plugin : Application.Plugins.Plugin
    {

        public Plugin(string path = null)
            : base(path)
        {
            //this.ColorOfConsole = ConsoleColor.Blue;
            //Console.ForegroundColor = this.ColorOfConsole;
            //Console.WriteLine("HELLO FROM NETWORK PLUGIN 1 CONSTRUCTOR");
        }

        //public override void DoSomething(params string[] parameters)
        //{

        //    foreach (string param in parameters)
        //    {
        //        Console.WriteLine(param.ToUpper());
        //    }
        //}
    }
}
