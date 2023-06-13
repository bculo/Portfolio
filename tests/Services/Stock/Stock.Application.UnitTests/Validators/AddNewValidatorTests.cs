using FluentAssertions;
using Stock.Application.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Application.UnitTests.Validators
{
    public class AddNewValidatorTests
    {
        private readonly AddNew.Validator _validator;

        public AddNewValidatorTests()
        {
            _validator = new AddNew.Validator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ValidateAsync_ShouldReturnListOfErrors_WhenSymbolIsNullOrEmpty(string symbol)
        {
            var command = new AddNew.Command { Symbol = symbol };

            var result = await _validator.ValidateAsync(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("123ABC")]
        [InlineData("A--1!")]
        public async Task ValidateAsync_ShouldReturnListOfErrors_WhenSymbolContainsInvalidCharacters(string symbol)
        {
            var command = new AddNew.Command { Symbol = symbol };

            var result = await _validator.ValidateAsync(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturnListOfErrors_WhenSymbolIsToLong()
        {
            var command = new AddNew.Command { Symbol = "VERYYYYLONGGSYMBOLLL" };

            var result = await _validator.ValidateAsync(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("TSLA")]
        [InlineData("AAPL")]
        public async Task ValidateAsync_ShouldReturnEmptyListOfErrors_WhenValidSymbolIsProvided(string symbol)
        {
            var command = new AddNew.Command { Symbol = symbol };

            var result = await _validator.ValidateAsync(command);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
