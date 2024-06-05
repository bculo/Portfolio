using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Tests.Common.Extensions;
using Time.Abstract.Contracts;
using User.Application.Features;

namespace User.Application.UnitTests.User;

public class RegisterUserDtoValidatorTests
{
    private const int YearGap = -30;
    private readonly Fixture _fixture = new Fixture().Configure();
    private readonly IDateTimeProvider _mockTimeProvider = Substitute.For<IDateTimeProvider>();

    public RegisterUserDtoValidatorTests()
    {
        _mockTimeProvider.Now.Returns(DateTime.UtcNow);
    }

    private RegisterUserDtoValidator GetValidator()
    {
        return new RegisterUserDtoValidator(_mockTimeProvider);
    }

    private RegisterUserDto GetValidDto()
    {
        return new RegisterUserDto()
        {
            UserName = "UserName",
            FirstName = "FirstName",
            LastName = "LastName",
            Password = "Password",
            Born = DateTime.UtcNow.AddYears(YearGap),
            Email = "test@mail.com"
        };
    }
    
    [Theory]
    [InlineData("tst")]
    [InlineData("")]
    [InlineData("    ")]
    public async Task ShouldReturnValidationErrors_WhenInvalidUserNameProvided(string userName)
    {
        var validator = GetValidator();
        var dto = GetValidDto();
        var request = dto with { UserName = userName };

        var result = await validator.ValidateAsync(request);
        result.IsValid.Should().Be(false);
        result.Errors.Should().Contain(x => x.PropertyName == nameof(RegisterUserDto.UserName));
    }
    
    [Theory]
    [InlineData("tst@")]
    [InlineData("")]
    [InlineData("    ")]
    public async Task ShouldReturnValidationErrors_WhenInvalidMailProvided(string mail)
    {
        var validator = GetValidator();
        var dto = GetValidDto();
        var request = dto with { Email = mail };

        var result = await validator.ValidateAsync(request);
        result.IsValid.Should().Be(false);
        result.Errors.Should().Contain(x => x.PropertyName == nameof(RegisterUserDto.Email));
    }
    
    [Fact]
    public async Task ShouldReturnValidationErrors_WhenPersonIsNotAdult()
    {
        var validator = GetValidator();
        var dto = GetValidDto();
        var request = dto with { Born = DateTime.UtcNow.AddYears(-17) };

        var result = await validator.ValidateAsync(request);
        result.IsValid.Should().Be(false);
        result.Errors.Should().Contain(x => x.PropertyName == nameof(RegisterUserDto.Born));
    }
}