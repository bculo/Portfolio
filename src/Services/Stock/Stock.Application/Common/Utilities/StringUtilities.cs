using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Application.Common.Utilities
{
    public static class StringUtilities
    {
        public static string AddStockPrefix(string word) 
        {
            return $"STOCK-{word}";
        }
    }
}
