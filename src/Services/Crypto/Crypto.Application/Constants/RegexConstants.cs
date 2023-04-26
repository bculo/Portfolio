using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crypto.Application.Constants
{
    public class RegexConstants
    {
        public static Regex SYMBOL = new Regex("^[a-zA-Z]{1,15}$", 
            RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled,
            TimeSpan.FromSeconds(1)); 
    }
}
