using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Sample.Plugin2
{
    public class Plugin : Application.Plugins.Plugin
    {
        
        public Plugin(string path)
            : base(path)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("HELLO FROM PLUGIN 2");
        }

    }
}
