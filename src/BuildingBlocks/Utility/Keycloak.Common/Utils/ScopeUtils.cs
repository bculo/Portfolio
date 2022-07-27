using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.Utils
{
    public static class ScopeUtils
    {
        public static string DefineScopesForRequest(IEnumerable<string> scopes)
        {
            if (scopes == null || !scopes.Any())
            {
                return null;
            }

            return string.Join(" ", scopes.Where(i => !string.IsNullOrEmpty(i)).Select(i => i?.Trim()));
        }
    }
}
