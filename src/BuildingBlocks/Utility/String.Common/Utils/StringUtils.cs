using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace String.Common.Utils
{
    public static class StringUtils
    {
        public static byte[]? StringToByteArray(string toConvert, string encodingType)
        {
            if (string.IsNullOrEmpty(toConvert) || string.IsNullOrEmpty(encodingType))
            {
                return null!;
            }

            var encoding = Encoding.GetEncoding(encodingType);

            return encoding.GetBytes(toConvert);
        }

        public static string ArrayToString(IEnumerable<string> words, string separator = "")
        {
            if(words is null || !words.Any())
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(separator))
            {
                throw new ArgumentNullException(nameof(separator));
            }

            return string.Join(separator, words);
        }
    }
}
