using System.Globalization;

namespace PrimitiveTypes.Common.Decimal
{
    public static class DecimalTypeUtilities
    {
        public static decimal ConvertToDecimal(string number, CultureInfo? cultureInfo = default)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return decimal.Zero;
            }

            CultureInfo useCulture = cultureInfo ?? CultureInfo.CurrentCulture;
            if (decimal.TryParse(number, useCulture, out decimal result))
            {
                return result;
            }

            return decimal.Zero;
        }

        public static decimal ToDecimal(this string number)
        {
            return ConvertToDecimal(number);
        }

        public static decimal? ConvertToNullableDecimal(string number, CultureInfo? cultureInfo = default)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return default;
            }

            CultureInfo useCulture = cultureInfo ?? CultureInfo.CurrentCulture;
            if (decimal.TryParse(number, useCulture, out decimal result))
            {
                return result;
            }

            return default;
        }

        public static decimal? ToNullableDecimal(this string number, CultureInfo? cultureInfo = default)
        {
            return ConvertToNullableDecimal(number, cultureInfo);
        }
    }
}
