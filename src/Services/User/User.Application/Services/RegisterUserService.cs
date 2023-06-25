using Auth0.Abstract.Contracts;
using Events.Common.User;
using FluentValidation;
using Keycloak.Common.Interfaces;
using Keycloak.Common.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using User.Application.Common.Exceptions;
using User.Application.Common.Options;
using User.Application.Entities;
using User.Application.Interfaces;
using User.Application.Persistence;

namespace User.Application.Services
{
    public class RegisterUserService : IRegisterUserService
    {
        private readonly UserDbContext _context;
        private readonly IPublishEndpoint _publish;
        private readonly KeycloakAdminOptions _options;
        private readonly IValidator<CreateUserDto> _validator;
        private readonly ILogger<RegisterUserService> _logger;
        private readonly IKeycloakAdminService _adminApiService;
        private readonly IAuth0OwnerCredentialFlowService _adminTokenService;

        public RegisterUserService(ILogger<RegisterUserService> logger,
            IValidator<CreateUserDto> validator,
            UserDbContext context,
            IAuth0OwnerCredentialFlowService adminTokenService,
            IOptionsSnapshot<KeycloakAdminOptions> options,
            IKeycloakAdminService adminApiService,
            IPublishEndpoint publish)
        {
            _validator = validator;
            _context = context;
            _logger = logger;
            _adminTokenService = adminTokenService;
            _adminApiService = adminApiService;
            _options = options.Value;
            _publish = publish;
        }

        public async Task RegisterUser(CreateUserDto userDto, CancellationToken token = default)
        {
            var validationResult = await _validator.ValidateAsync(userDto, token).ConfigureAwait(false);
            if(!validationResult.IsValid)
            {
                var errors = validationResult.ToDictionary();
                _logger.LogTrace(JsonConvert.SerializeObject(errors));
                throw new PortfolioUserValidationException(errors);
            }

            var adminTokenResponse = await _adminTokenService.GetToken(_options.ClientId, _options.UserName, _options.Password).ConfigureAwait(false);
            if(adminTokenResponse is null)
            {
                throw new PortfolioUserCoreException(
                    "Problem occurrd in process of fetching admin access token from keycloak",
                    "An error occurred. Please try again later.");
            }

            var keyCloakModel = MapToKeycloakModel(userDto);
            if(!await _adminApiService.CreateUser(_options.Realm, adminTokenResponse.AccessToken, keyCloakModel).ConfigureAwait(false))
            {
                throw new PortfolioUserCoreException(
                    "Couldn't add new user via Keycloak admin API",
                    "An error occurred. Please try again later.");
            }

            var entity = MapToEntityModel(userDto);

            try
            {
                _context.Users.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch
            {
                await _publish.Publish(MapToEvent(userDto));
                throw;
            }
        }

        private UserRepresentation MapToKeycloakModel(CreateUserDto userDto)
        {
            return new UserRepresentation
            {
                Enabled = true,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                UserName = userDto.UserName,
                Id = Guid.NewGuid().ToString(),
                RealmRoles = new string[] { "User" },
                Credentials = new CredentialRepresentation[]
                {
                    new CredentialRepresentation
                    {
                        Temporary = false,
                        Type = "password",
                        Value = userDto.Password
                    }
                },
                Email = userDto.Email,
            };
        }

        private PortfolioUser MapToEntityModel(CreateUserDto userDto)
        {
            return new PortfolioUser
            {
                BornOn = userDto.Born,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                UserName = userDto.UserName,
            };
        }

        private UserNotSavedToPersistenceStorage MapToEvent(CreateUserDto userDto)
        {
            return new UserNotSavedToPersistenceStorage
            {
                BornOn = userDto.Born,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                UserName = userDto.UserName,
            };
        }

        public async Task AddUserToStorage(UserBaseInfoDto instance, CancellationToken token = default)
        {
            var entity = MapToEntityModel(instance);
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
        }

        private PortfolioUser MapToEntityModel(UserBaseInfoDto userDto)
        {
            return new PortfolioUser
            {
                BornOn = userDto.Born,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                UserName = userDto.UserName,
            };
        }
    }
}
