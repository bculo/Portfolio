using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace String.Common.Extensions
{
    public static class StringExtensions
    {
        public static string TrimUpper(this string characters)
        {
            return characters?.Trim()?.ToUpper() ?? null;
        }

        public static string TrimDown(this string characters)
        {
            return characters?.Trim()?.ToUpper() ?? null;
        }
    }
}
