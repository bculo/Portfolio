using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tests.Common.FixtureUtilities;
using Time.Abstract.Contracts;
using User.Application.Entities;
using User.Application.Interfaces;
using User.Application.Persistence;
using User.Application.Validators;

namespace User.Application.UnitTests.Validators
{
    public class CreateUserDtoValidatorTests
    {
        private readonly Fixture _fixture = FixtureHelper.FixtureCircularBehavior();
        private readonly Mock<IDateTimeProvider> _mockTimeProvider = new Mock<IDateTimeProvider>();

        public CreateUserDtoValidatorTests()
        {
            _mockTimeProvider.Setup(x => x.Now).Returns(DateTime.UtcNow);
        }

        [Theory]
        [InlineData("tst")]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public async Task ValidateAsync_ShouldReturnValidationErrors_WhenInvalidUserNameProvided(string username)
        {
            var items = GetPortfolioUsers();
            var mock = items.AsQueryable().BuildMockDbSet();

            var contextMock = new Mock<UserDbContext>();
            contextMock.Setup(x => x.Users).Returns(mock.Object);
          
            var request = _fixture.Build<CreateUserDto>()
                .With(x => x.UserName, username)
                .With(x => x.Email, "test@gmail.com")
                .With(x => x.Born, DateTime.UtcNow.AddYears(-20))
                .Create();

            var validator = new CreateUserDtoValidator(_mockTimeProvider.Object, contextMock.Object);
            var result = await validator.ValidateAsync(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0)
                .And.Contain(x => x.PropertyName == "UserName");
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public async Task ValidateAsync_ShouldReturnValidationErrors_WhenInvalidFirstNameProvided(string firstName)
        {
            var items = GetPortfolioUsers();
            var mock = items.AsQueryable().BuildMockDbSet();

            var contextMock = new Mock<UserDbContext>();
            contextMock.Setup(x => x.Users).Returns(mock.Object);

            var request = _fixture.Build<CreateUserDto>()
                .With(x => x.FirstName, firstName)
                .With(x => x.Email, "test@gmail.com")
                .With(x => x.Born, DateTime.UtcNow.AddYears(-20))
                .Create();

            var validator = new CreateUserDtoValidator(_mockTimeProvider.Object, contextMock.Object);
            var result = await validator.ValidateAsync(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0)
                .And.Contain(x => x.PropertyName == "FirstName");
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public async Task ValidateAsync_ShouldReturnValidationErrors_WhenInvalidLastNameProvided(string lastName)
        {
            var items = GetPortfolioUsers();
            var mock = items.AsQueryable().BuildMockDbSet();

            var contextMock = new Mock<UserDbContext>();
            contextMock.Setup(x => x.Users).Returns(mock.Object);

            var request = _fixture.Build<CreateUserDto>()
                .With(x => x.LastName, lastName)
                .With(x => x.Email, "test@gmail.com")
                .With(x => x.Born, DateTime.UtcNow.AddYears(-20))
                .Create();

            var validator = new CreateUserDtoValidator(_mockTimeProvider.Object, contextMock.Object);
            var result = await validator.ValidateAsync(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0)
                .And.Contain(x => x.PropertyName == "LastName");
        }

        [Theory]
        [InlineData(5)]
        [InlineData(14)]
        public async Task ValidateAsync_ShouldReturnValidationErrors_WhenPersonIsNotAdult(int age)
        {
            var items = GetPortfolioUsers();
            var mock = items.AsQueryable().BuildMockDbSet();

            var contextMock = new Mock<UserDbContext>();
            contextMock.Setup(x => x.Users).Returns(mock.Object);

            var request = _fixture.Build<CreateUserDto>()
                .With(x => x.Born, DateTime.UtcNow.AddYears(-age))
                .With(x => x.Email, "test@gmail.com")
                .Create();

            var validator = new CreateUserDtoValidator(_mockTimeProvider.Object, contextMock.Object);
            var result = await validator.ValidateAsync(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0)
                .And.Contain(x => x.PropertyName == "Born");
        }

        [Theory]
        [InlineData("@gmail.com")]
        [InlineData("@gmail")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("brrr")]
        [InlineData("tst@tst")]
        public async Task ValidateAsync_ShouldReturnValidationErrors_WhenInvalidEmailProvided(string email)
        {
            var items = GetPortfolioUsers();
            var mock = items.AsQueryable().BuildMockDbSet();

            var contextMock = new Mock<UserDbContext>();
            contextMock.Setup(x => x.Users).Returns(mock.Object);

            var request = _fixture.Build<CreateUserDto>()
                .With(x => x.Born, DateTime.UtcNow.AddYears(-20))
                .With(x => x.Email, email)
                .Create();

            var validator = new CreateUserDtoValidator(_mockTimeProvider.Object, contextMock.Object);
            var result = await validator.ValidateAsync(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0)
                .And.Contain(x => x.PropertyName == "Email");
        }

        [Fact]
        public async Task ValidateAsync_ShouldReturnValidationErrors_WhenUsernameIsNotUnique()
        {
            string existingUserName = "tester";

            var existingItem = _fixture.Build<PortfolioUser>()
                .With(x => x.UserName, existingUserName)
                .Create();

            var items = GetPortfolioUsers();
            items.Add(existingItem);
            var mock = items.AsQueryable().BuildMockDbSet();

            var contextMock = new Mock<UserDbContext>();
            contextMock.Setup(x => x.Users).Returns(mock.Object);

            var request = _fixture.Build<CreateUserDto>()
                .With(x => x.Born, DateTime.UtcNow.AddYears(-20))
                .With(x => x.Email, "test@gmail.com")
                .With(x => x.UserName, existingUserName)
                .Create();

            var validator = new CreateUserDtoValidator(_mockTimeProvider.Object, contextMock.Object);
            var result = await validator.ValidateAsync(request);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCountGreaterThan(0)
                .And.Contain(x => x.PropertyName == "UserName");
        }

        private List<PortfolioUser> GetPortfolioUsers()
        {
            return _fixture.CreateMany<PortfolioUser>(10).ToList();
        }
    }
}
