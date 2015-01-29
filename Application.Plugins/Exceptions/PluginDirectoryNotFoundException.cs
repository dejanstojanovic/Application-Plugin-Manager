using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Application.Plugins.Exceptions
{
    [Serializable]
    public class PluginDirectoryNotFoundException : System.IO.IOException
    {
        public PluginDirectoryNotFoundException()
            : base()
        {

        }

        public PluginDirectoryNotFoundException(string message)
            : base(message) { }

        public PluginDirectoryNotFoundException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public PluginDirectoryNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        public PluginDirectoryNotFoundException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected PluginDirectoryNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
