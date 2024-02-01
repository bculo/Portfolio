using System.Globalization;

namespace PrimitiveTypes.Common.Decimal
{
    public static class DecimalTypeUtilities
    {
        public static decimal ConvertToDecimal(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return decimal.Zero;
            }

            if (decimal.TryParse(number, CultureInfo.CurrentCulture, out decimal result))
            {
                return result;
            }

            return decimal.Zero;
        }

        public static decimal ToDecimal(this string number)
        {
            return ConvertToDecimal(number);
        }

        public static decimal? ConvertToNullableDecimal(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return default;
            }

            if (decimal.TryParse(number, CultureInfo.CurrentCulture, out decimal result))
            {
                return result;
            }

            return default;
        }

        public static decimal? ToNullableDecimal(this string number)
        {
            return ConvertToNullableDecimal(number);
        }
    }
}
