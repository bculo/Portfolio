using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Common.Helpers
{
    public static class CachePrefixHelper
    {
        public static string DefinePrefix(string identifier)
        {
            ArgumentNullException.ThrowIfNull(identifier);

            return $"CRYPTO-{identifier.Trim()}";
        }
    }
}
