using BultInTypes.Common.Decimal;
using BultInTypes.Common.Float;
using FluentAssertions;
using System.Globalization;

namespace BultInTypes.Common.UnitTests.Decimal
{
    public class DecimalTypeUtilitiesTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ConverToDecimal_ShouldReturnDefaultValue_WhenNullOrEmptyStringProvided(string value)
        {
            var result = DecimalTypeUtilities.ConvertToDecimal(value);
            result.Should().Be(0m);
        }

        [Theory]
        [InlineData("123.25")]
        [InlineData("-15.3")]
        [InlineData("1,000.3")]
        [InlineData("-15,000.3")]
        [InlineData("-15000.3")]
        [InlineData("25000.33")]
        public void ConverToDecimal_ShouldReturnGivenNumberAsFloat_WhenDecimalPointIsDot(string value)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            var result = DecimalTypeUtilities.ConvertToDecimal(value);
            result.Should().NotBe(0m);
        }

        [Theory]
        [InlineData("123,25")]
        [InlineData("-15,3")]
        [InlineData("-15000,3")]
        [InlineData("25000,33")]
        public void ConverToDecimal_ShouldReturnGivenNumberAsFloat_WhenDecimalPointIsComma(string value)
        {
            CultureInfo.CurrentCulture = new CultureInfo("hr-HR");
            var result = DecimalTypeUtilities.ConvertToDecimal(value);
            result.Should().NotBe(0m);
        }
    }
}
