using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace String.Common.Utils
{
    public static class StringUtils
    {
        public static byte[] StringToByteArray(string toConvert, string encodingType)
        {
            if (string.IsNullOrEmpty(toConvert))
            {
                return null!;
            }

            var encoding = Encoding.GetEncoding(encodingType);

            return encoding.GetBytes(toConvert);
        }
    }
}
