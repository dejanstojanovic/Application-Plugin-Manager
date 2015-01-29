using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Plugins.Caching
{
    /// <summary>
    /// Plugin cache type enumeration
    /// </summary>
    [Flags]
    public enum CachePolicyType
    {
        FileWatch = 1,
        TimeInterval = 2
    }
}
