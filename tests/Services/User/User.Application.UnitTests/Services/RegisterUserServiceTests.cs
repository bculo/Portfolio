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
        private Mock<UserDbContext> _dbContext = new Mock<UserDbContext>();
        private Mock<IPublishEndpoint> _endpointMock = new Mock<IPublishEndpoint>();
        private Mock<IKeycloakAdminService> _adminApi = new Mock<IKeycloakAdminService>();
        private ILogger<RegisterUserService> _logger = Mock.Of<ILogger<RegisterUserService>>();
        private Mock<IValidator<CreateUserDto>> _validator = new Mock<IValidator<CreateUserDto>>();
        private Mock<IAuth0OwnerCredentialFlowService> _flowService = new Mock<IAuth0OwnerCredentialFlowService>();
        private Mock<IOptionsSnapshot<KeycloakAdminOptions>> _options = new Mock<IOptionsSnapshot<KeycloakAdminOptions>>();

        public RegisterUserServiceTests()
        {
            _options.Setup(x => x.Value).Returns(new KeycloakAdminOptions
            {
                ClientId = "testclientid",
                Password = "password",
                Realm = "testrealm",
                UserName = "username",
            });
        }

        [Fact]
        public async Task RegisterUser_ShouldThrowException_WhenInvalidDtoPassed()
        {
            _validator.Setup(x => x.ValidateAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult 
                { 
                    Errors = _fixture.CreateMany<ValidationFailure>(3).ToList() 
                });

            var service = new RegisterUserService(_logger, 
                _validator.Object, _dbContext.Object, _flowService.Object,
                _options.Object, _adminApi.Object, _endpointMock.Object);

            var invalidRequest = _fixture.Build<CreateUserDto>()
                .With(x => x.Email, "invalidmail")
                .Create();

            await Assert.ThrowsAsync<ArgumentException>(() => service.RegisterUser(invalidRequest, CancellationToken.None));
            _endpointMock.Verify(x => x.Publish(It.IsAny<UserNotSavedToPersistenceStorage>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public async Task RegisterUser_ShouldThrowException_WhenKeycloakAdminTokenIsNotFetched()
        {
            _validator.Setup(x => x.ValidateAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult
                {
                    Errors = new List<ValidationFailure>()
                });

            _flowService.Setup(x => x.GetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync((TokenAuthorizationCodeResponse)null!);

            var service = new RegisterUserService(_logger,
                _validator.Object, _dbContext.Object, _flowService.Object,
                _options.Object, _adminApi.Object, _endpointMock.Object);

            var validRequest = _fixture.Create<CreateUserDto>();

            await Assert.ThrowsAsync<ArgumentException>(() => service.RegisterUser(validRequest, CancellationToken.None));
            _endpointMock.Verify(x => x.Publish(It.IsAny<UserNotSavedToPersistenceStorage>(), It.IsAny<CancellationToken>()), Times.Never());
        }


        [Fact]
        public async Task RegisterUser_ShouldThrowException_WhenUserIsNotAddedSuccessfullyToKeycloak()
        {
            _validator.Setup(x => x.ValidateAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult
                {
                    Errors = new List<ValidationFailure>()
                });

            _flowService.Setup(x => x.GetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync(_fixture.Create<TokenAuthorizationCodeResponse>());

            _adminApi.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserRepresentation>()))
                .ReturnsAsync(false);

            var service = new RegisterUserService(_logger,
                _validator.Object, _dbContext.Object, _flowService.Object,
                _options.Object, _adminApi.Object, _endpointMock.Object);

            var validRequest = _fixture.Create<CreateUserDto>();

            await Assert.ThrowsAsync<ArgumentException>(() => service.RegisterUser(validRequest, CancellationToken.None));
            _endpointMock.Verify(x => x.Publish(It.IsAny<UserNotSavedToPersistenceStorage>(), It.IsAny<CancellationToken>()), Times.Never());
        }

        [Fact]
        public async Task RegisterUser_ShouldThrowException_WhenFailureOccurOnAddingNewUserToStorage()
        {
            _validator.Setup(x => x.ValidateAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult
                {
                    Errors = new List<ValidationFailure>()
                });

            _flowService.Setup(x => x.GetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync(_fixture.Create<TokenAuthorizationCodeResponse>());

            _adminApi.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserRepresentation>()))
                .ReturnsAsync(true);

            var usersDbSetMock = new List<PortfolioUser>().AsQueryable().BuildMockDbSet();
            _dbContext.Setup(x => x.Users).Returns(usersDbSetMock.Object);
            _dbContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Throws(new DbUpdateException());

            var service = new RegisterUserService(_logger,
                _validator.Object, _dbContext.Object, _flowService.Object,
                _options.Object, _adminApi.Object, _endpointMock.Object);

            var validRequest = _fixture.Create<CreateUserDto>();

            await Assert.ThrowsAsync<DbUpdateException>(() => service.RegisterUser(validRequest, CancellationToken.None));
            _endpointMock.Verify(x => x.Publish(It.IsAny<UserNotSavedToPersistenceStorage>(), It.IsAny<CancellationToken>()), Times.Once());
        }


        [Fact]
        public async Task RegisterUser_ShouldExecuteWithoutException_WhenAllInnerServicesExecuteWithoutFailure()
        {
            _validator.Setup(x => x.ValidateAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult
                {
                    Errors = new List<ValidationFailure>()
                });

            _flowService.Setup(x => x.GetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync(_fixture.Create<TokenAuthorizationCodeResponse>());

            _adminApi.Setup(x => x.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserRepresentation>()))
                .ReturnsAsync(true);

            var usersDbSetMock = new List<PortfolioUser>().AsQueryable().BuildMockDbSet();
            _dbContext.Setup(x => x.Users).Returns(usersDbSetMock.Object);

            var service = new RegisterUserService(_logger,
                _validator.Object, _dbContext.Object, _flowService.Object,
                _options.Object, _adminApi.Object, _endpointMock.Object);

            var validRequest = _fixture.Create<CreateUserDto>();

            await service.RegisterUser(validRequest, CancellationToken.None);

            _endpointMock.Verify(x => x.Publish(It.IsAny<UserNotSavedToPersistenceStorage>(), It.IsAny<CancellationToken>()), Times.Never());
        }
    }
}
