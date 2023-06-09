using BultInTypes.Common.Float;
using FluentAssertions;
using System.Globalization;

namespace BultInTypes.Common.UnitTests.Float
{
    public class FloatTypeUtilitiesTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ConvertToFloatt_ShouldReturnDefaultValue_WhenNullOrEmptyStringProvided(string value)
        {
            var result = FloatTypeUtilities.ConvertToFloat(value);
            result.Should().Be(0f);
        }

        [Theory]
        [InlineData("123.25")]
        [InlineData("-15.3")]
        [InlineData("1,000.3")]
        [InlineData("-15,000.3")]
        [InlineData("-15000.3")]
        [InlineData("25000.33")]
        public void ConvertToFloat_ShouldReturnGivenNumberAsFloat_WhenDecimalPointIsDot(string value)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            var result = FloatTypeUtilities.ConvertToFloat(value);
            result.Should().NotBe(0f);
        }

        [Theory]
        [InlineData("123,25")]
        [InlineData("-15,3")]
        [InlineData("-15000,3")]
        [InlineData("25000,33")]
        public void ConvertToFloat_ShouldReturnGivenNumberAsFloat_WhenDecimalPointIsComma(string value)
        {
            CultureInfo.CurrentCulture = new CultureInfo("hr-HR");
            var result = FloatTypeUtilities.ConvertToFloat(value);
            result.Should().NotBe(0f);
        }
    }
}
