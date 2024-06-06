using System.Text.RegularExpressions;

namespace Crypto.Application.Common.Constants
{
    public static class RegexConstants
    {
        public static readonly Regex Symbol = new Regex("^[a-zA-Z]{1,15}$", 
            RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled,
            TimeSpan.FromSeconds(1)); 
    }
}
