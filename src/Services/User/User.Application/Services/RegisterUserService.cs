using Auth0.Abstract.Contracts;
using Auth0.Abstract.Models;
using Events.Common.User;
using FluentValidation;
using Keycloak.Common.Interfaces;
using Keycloak.Common.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        /// <summary>
        /// Execute registration procedure for a new user
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task RegisterUser(CreateUserDto userDto, CancellationToken token = default)
        {
            await ValidateInstance(userDto, token);

            var adminTokenResponse = await FetchKeycloakAdminAccessToken();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //Add user to DB
                var entity = MapToEntityModel(userDto);
                _context.Users.Add(entity);
                await _context.SaveChangesAsync();

                //Add user to keycloak
                await AddUserToKeycloak(userDto, adminTokenResponse.AccessToken);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            await _publish.Publish(new NewPorfolioUserRegistered
            {
                UserName = userDto.UserName,
            });
        }

        /// <summary>
        /// Validate received DTO with fluent validation
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="PortfolioUserValidationException">Method throws exception if dto is invalid</exception>
        private async Task ValidateInstance(CreateUserDto userDto, CancellationToken token)
        {
            var validationResult = await _validator.ValidateAsync(userDto, token)
                .ConfigureAwait(false);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.ToDictionary();
                _logger.LogTrace(JsonConvert.SerializeObject(errors));
                throw new PortfolioUserValidationException(errors);
            }
        }

        /// <summary>
        /// Try fetch admin access token that is nedded for admin API usage
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PortfolioUserCoreException"></exception>
        private async Task<TokenAuthorizationCodeResponse> FetchKeycloakAdminAccessToken()
        {
            var adminTokenResponse = await _adminTokenService.GetToken(_options.ClientId, _options.UserName, _options.Password)
                .ConfigureAwait(false);

            if (adminTokenResponse is null)
            {
                throw new PortfolioUserCoreException(
                    "Problem occurrd in process of fetching admin access token from keycloak",
                    "An error occurred. Please try again later.");
            }

            return adminTokenResponse;
        }

        /// <summary>
        /// Add user to keycloak storage via ADMIN API
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        /// <exception cref="PortfolioUserCoreException"></exception>
        private async Task AddUserToKeycloak(CreateUserDto userDto, string accessToken)
        {
            var keyCloakModel = MapToKeycloakModel(userDto);
            if (!await _adminApiService.CreateUser(_options.Realm, accessToken, keyCloakModel).ConfigureAwait(false))
            {
                throw new PortfolioUserCoreException(
                    "Couldn't add new user via Keycloak admin API",
                    "An error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Map received DTO model to keycloak model (request keycloak model)
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Map received DTO model to entity model
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
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
    }
}
