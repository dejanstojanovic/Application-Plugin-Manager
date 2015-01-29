using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Application.Plugins.Exceptions
{
    [Serializable]
    public class PluginFileNotInPluginsFolderException : System.IO.IOException
    {
        public PluginFileNotInPluginsFolderException()
            : base()
        {

        }

        public PluginFileNotInPluginsFolderException(string message)
            : base(message) { }

        public PluginFileNotInPluginsFolderException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public PluginFileNotInPluginsFolderException(string message, Exception innerException)
            : base(message, innerException) { }

        public PluginFileNotInPluginsFolderException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected PluginFileNotInPluginsFolderException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
