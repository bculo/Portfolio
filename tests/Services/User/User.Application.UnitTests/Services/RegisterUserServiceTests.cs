using Auth0.Abstract.Contracts;
using Auth0.Abstract.Models;
using AutoFixture;
using Events.Common.User;
using FluentValidation;
using FluentValidation.Results;
using Keycloak.Common.Interfaces;
using Keycloak.Common.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Tests.Common.EntityFramework;
using Time.Abstract.Contracts;
using User.Application.Common.Exceptions;
using User.Application.Common.Options;
using User.Application.Entities;
using User.Application.Interfaces;
using User.Application.Persistence;
using User.Application.Services;
using Xunit;

namespace User.Application.UnitTests.Services
{
    public class RegisterUserServiceTests
    {
        private Fixture _fixture = new Fixture();
        private Mock<UserDbContext> _dbContextMock = new Mock<UserDbContext>();
        private Mock<IPublishEndpoint> _endpointMock = new Mock<IPublishEndpoint>();
        private Mock<IDateTimeProvider> _timeProvider = new Mock<IDateTimeProvider>();
        private Mock<IKeycloakAdminService> _adminApiMock = new Mock<IKeycloakAdminService>();
        private Mock<IValidator<CreateUserDto>> _validatorMock = new Mock<IValidator<CreateUserDto>>();
        private Mock<ILogger<RegisterUserService>> _loggerMock = new Mock<ILogger<RegisterUserService>>();
        private Mock<IAuth0OwnerCredentialFlowService> _flowServiceMock = new Mock<IAuth0OwnerCredentialFlowService>();
        private Mock<IOptionsSnapshot<KeycloakAdminOptions>> _optionsMock = new Mock<IOptionsSnapshot<KeycloakAdminOptions>>();

        public RegisterUserServiceTests()
        {
            _optionsMock.Setup(x => x.Value).Returns(new KeycloakAdminOptions
            {
                ClientId = "testclientid",
                Password = "password",
                Realm = "testrealm",
                UserName = "username",
            });

            _timeProvider.Setup(x => x.Now).Returns(DateTime.UtcNow);
        }

        [Fact]
        public async Task RegisterUser_ShouldThrowException_WhenInvalidDtoPassed()
        {
            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult 
                { 
                    Errors = _fixture.CreateMany<ValidationFailure>(3).ToList() 
                });

            var service = new RegisterUserService(_loggerMock.Object, 
                _validatorMock.Object, _dbContextMock.Object, _flowServiceMock.Object,
                _optionsMock.Object, _adminApiMock.Object, _endpointMock.Object, _timeProvider.Object);

            var invalidRequest = _fixture.Build<CreateUserDto>()
                .With(x => x.Email, "invalidmail")
                .Create();

            await Assert.ThrowsAsync<PortfolioUserValidationException>(() => service.RegisterUser(invalidRequest, CancellationToken.None));
            _endpointMock.Verify(x => x.Publish(It.IsAny<PortfolioUserApproved>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public async Task RegisterUser_ShouldThrowException_WhenKeycloakAdminTokenIsNotFetched()
        {
            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult
                {
                    Errors = new List<ValidationFailure>()
                });

            _flowServiceMock.Setup(x => x.GetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync((TokenAuthorizationCodeResponse)null!);

            var service = new RegisterUserService(_loggerMock.Object,
                _validatorMock.Object, _dbContextMock.Object, _flowServiceMock.Object,
                _optionsMock.Object, _adminApiMock.Object, _endpointMock.Object, _timeProvider.Object);

            var validRequest = _fixture.Create<CreateUserDto>();

            await Assert.ThrowsAsync<PortfolioUserCoreException>(() => service.RegisterUser(validRequest, CancellationToken.None));
            _endpointMock.Verify(x => x.Publish(It.IsAny<PortfolioUserApproved>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public async Task RegisterUser_ShouldThrowException_WhenUserIsNotAddedSuccessfullyToKeycloak()
        {
            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult
                {
                    Errors = new List<ValidationFailure>()
                });

            _flowServiceMock.Setup(x => x.GetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync(_fixture.Create<TokenAuthorizationCodeResponse>());

            _adminApiMock.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserRepresentation>()))
                .ReturnsAsync(false);

            var usersDbSetMock = new List<PortfolioUser>().AsQueryable().BuildMockDbSet();
            _dbContextMock.Setup(x => x.Users).Returns(usersDbSetMock.Object);
            _dbContextMock.SetupGet(x => x.Database).Returns(new MockDatabaseFacade(_dbContextMock.Object));

            var service = new RegisterUserService(_loggerMock.Object,
                _validatorMock.Object, _dbContextMock.Object, _flowServiceMock.Object,
                _optionsMock.Object, _adminApiMock.Object, _endpointMock.Object, _timeProvider.Object);

            var validRequest = _fixture.Create<CreateUserDto>();

            await Assert.ThrowsAsync<PortfolioUserCoreException>(() => service.RegisterUser(validRequest, CancellationToken.None));
            _endpointMock.Verify(x => x.Publish(It.IsAny<PortfolioUserApproved>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public async Task RegisterUser_ShouldThrowException_WhenFailureOccurOnAddingNewUserToStorage()
        {
            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult
                {
                    Errors = new List<ValidationFailure>()
                });

            _flowServiceMock.Setup(x => x.GetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync(_fixture.Create<TokenAuthorizationCodeResponse>());

            _adminApiMock.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserRepresentation>()))
                .ReturnsAsync(true);

            var usersDbSetMock = new List<PortfolioUser>().AsQueryable().BuildMockDbSet();
            _dbContextMock.Setup(x => x.Users).Returns(usersDbSetMock.Object);
            _dbContextMock.SetupGet(x => x.Database).Returns(new MockDatabaseFacade(_dbContextMock.Object));
            _dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Throws(new DbUpdateException());

            var service = new RegisterUserService(_loggerMock.Object,
                _validatorMock.Object, _dbContextMock.Object, _flowServiceMock.Object,
                _optionsMock.Object, _adminApiMock.Object, _endpointMock.Object, _timeProvider.Object);

            var validRequest = _fixture.Create<CreateUserDto>();

            await Assert.ThrowsAsync<DbUpdateException>(() => service.RegisterUser(validRequest, CancellationToken.None));
        }

        [Fact]
        public async Task RegisterUser_ShouldExecuteWithoutException_WhenAllInnerServicesExecuteWithoutFailure()
        {
            _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult
                {
                    Errors = new List<ValidationFailure>()
                });

            _flowServiceMock.Setup(x => x.GetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync(_fixture.Create<TokenAuthorizationCodeResponse>());

            _adminApiMock.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserRepresentation>()))
                .ReturnsAsync(true);

            var usersDbSetMock = new List<PortfolioUser>().AsQueryable().BuildMockDbSet();
            _dbContextMock.SetupGet(x => x.Database).Returns(new MockDatabaseFacade(_dbContextMock.Object));
            _dbContextMock.Setup(x => x.Users).Returns(usersDbSetMock.Object);

            var service = new RegisterUserService(_loggerMock.Object,
                _validatorMock.Object, _dbContextMock.Object, _flowServiceMock.Object,
                _optionsMock.Object, _adminApiMock.Object, _endpointMock.Object, _timeProvider.Object);

            var validRequest = _fixture.Create<CreateUserDto>();

            await service.RegisterUser(validRequest, CancellationToken.None);

            _endpointMock.Verify(x => x.Publish(It.IsAny<PortfolioUserApproved>(), It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}
