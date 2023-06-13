using FluentAssertions;
using Stock.Application.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Application.UnitTests.Validators
{
    public class UpdateBatchValidatorTests
    {
        private readonly UpdateBatch.Validator validator = new UpdateBatch.Validator();

        [Fact]
        public async Task ValidateAsync_ShouldReturnErrors_WhenNullGiven()
        {
            var command = new UpdateBatch.Command
            {
                Symbols = null
            };

            var result = await validator.ValidateAsync(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
        }


        [Fact]
        public async Task ValidateAsync_ShouldReturnErrors_WhenEmptySymbolsListGiven()
        {
            var command = new UpdateBatch.Command
            {
                Symbols = new List<string>()
            };

            var result = await validator.ValidateAsync(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturnErrors_WhenOneElementOfArrayIsEmpty()
        {
            var command = new UpdateBatch.Command
            {
                Symbols = new List<string>()
                {
                    "A", string.Empty, "C", "D"
                }
            };

            var result = await validator.ValidateAsync(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturnErrors_WhenOneElementOfArrayIsNull()
        {
            var command = new UpdateBatch.Command
            {
                Symbols = new List<string>()
                {
                    "A", null, "C", "D"
                }
            };

            var result = await validator.ValidateAsync(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task ValidateAsync_ShouldBeValid_WhenAllSymbolsValid()
        {
            var command = new UpdateBatch.Command
            {
                Symbols = new List<string>()
                {
                    "A", "R", "C", "D"
                }
            };

            var result = await validator.ValidateAsync(command);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().HaveCount(0);
        }
    }
}
