using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Application.Plugins.Exceptions
{
    [Serializable]
    public class PluginFileNotFoundException : System.IO.IOException
    {
        public PluginFileNotFoundException()
            : base()
        {

        }

        public PluginFileNotFoundException(string message)
            : base(message) { }

        public PluginFileNotFoundException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public PluginFileNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        public PluginFileNotFoundException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected PluginFileNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
