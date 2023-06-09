using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BultInTypes.Common.Float
{
    public static class FloatTypeUtilities
    {
        public static float ConvertToFloat(string number)
        {
            if(string.IsNullOrWhiteSpace(number))
            {
                return 0f;
            }

            if(float.TryParse(number, CultureInfo.CurrentCulture, out float result))
            {
                return result;
            }

            return 0f;
        }

        public static float ToFloat(this string number)
        {
            return ConvertToFloat(number);
        }

        public static float? ConvertToNullableFloat(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return default;
            }

            if (float.TryParse(number, CultureInfo.CurrentCulture, out float result))
            {
                return result;
            }

            return default;
        }

        public static float? ToNullableFloat(this string number)
        {
            return ConvertToNullableFloat(number);
        }
    }
}
