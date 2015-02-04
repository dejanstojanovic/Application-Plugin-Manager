using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Sample.Plugin1.Graphics
{
    public class Plugin : Application.Sample.Base.BasePlugin
    {

        public Plugin(string path = null)
            : base(path)
        {
            //this.ColorOfConsole = ConsoleColor.Green;
            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine(string.Format("HELLO FROM GRAPHIC PLUGIN 1 CONSTRUCTOR {0}",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")));
        }

        public override void DoSomething(params string[] parameters)
        {
            foreach (string param in parameters)
            {
                //Console.WriteLine(param.ToLower());
            }
        }

    }
}
