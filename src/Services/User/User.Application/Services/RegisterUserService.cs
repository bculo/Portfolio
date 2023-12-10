using Auth0.Abstract.Contracts;
using Events.Common.User;
using Keycloak.Common.Models;
using Keycloak.Common.Options;
using Keycloak.Common.Refit;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Time.Abstract.Contracts;
using User.Application.Common.Exceptions;
using User.Application.Entities;
using User.Application.Interfaces;
using User.Application.Persistence;

namespace User.Application.Services
{
    public class RegisterUserService : IRegisterUserService
    {
        private readonly UserDbContext _context;
        private readonly IPublishEndpoint _publish;
        private readonly KeycloakAdminApiOptions _config;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IUsersApi _userClient;

        public RegisterUserService(UserDbContext context,
            IUsersApi userClient,
            IPublishEndpoint publish,
            IDateTimeProvider timeProvider,
            IOptions<KeycloakAdminApiOptions> config)
        {
            _context = context;
            _userClient = userClient;
            _publish = publish;
            _timeProvider = timeProvider;
            _config = config.Value;
        }

        /// <summary>
        /// Execute registration procedure for a new user
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task RegisterUser(CreateUserDto userDto, CancellationToken token = default)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(token);
            try
            {
                var entity = MapToEntityModel(userDto);
                _context.Users.Add(entity);
                await _context.SaveChangesAsync(token).ConfigureAwait(false);
                
                var keyCloakModel = MapToKeycloakModel(userDto);
                var response = await _userClient.PostUsers(_config.Realm, keyCloakModel).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    // DO SOMETHING
                }
                
                await transaction.CommitAsync(token);
            }
            catch(Exception e)
            {
                await transaction.RollbackAsync(token);
                throw;
            }

            await _publish.Publish(new NewPorfolioUserRegistered
            {
                UserName = userDto.UserName,
            }, token);
        }
        
        
        private async Task UpdateKeyCloakUser(UserRepresentation keycloakUpdateRequest, string userId)
        {
            var response = await _userClient.PutUser(_config.Realm, userId, keycloakUpdateRequest)
                .ConfigureAwait(false);
            
            if(!response.IsSuccessStatusCode)
            {
                throw new PortfolioUserCoreException(
                    "Couldn't update existing Keycloak user via admin API",
                    "An error occurred. Please try again later.");
            }
        }
        
        private UserRepresentation MapToKeycloakModel(CreateUserDto userDto)
        {
            return new UserRepresentation
            {
                Enabled = false,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Username = userDto.UserName,
                Id = Guid.NewGuid().ToString(),
                RealmRoles = new[] { "User" },
                Credentials = new[]
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

        /// <summary>
        /// Approve user and add keycloak user id to db user instance 
        /// If user has externalid value, that means that he is confirmed by the admin
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="PortfolioUserNotFoundException"></exception>
        public async Task ApproveUser(long userId, CancellationToken token = default)
        {
            var dbUser = await _context.Users.FindAsync(userId);
            if(dbUser is null)
            {
                throw new PortfolioUserNotFoundException("User not found");
            }
            
            var keycloakUser = await FetchKeycloakUserByUniqueUserName(dbUser.UserName);
            await using var transaction = await _context.Database.BeginTransactionAsync(token);
            try
            {
                dbUser.ExternalId = Guid.Parse(keycloakUser.Id);
                await _context.SaveChangesAsync(token);

                Console.WriteLine(JsonConvert.SerializeObject(keycloakUser));
                
                var keycloakUpdateRequest = new UserRepresentation
                {
                    Enabled = true,
                };

                await UpdateKeyCloakUser(keycloakUpdateRequest, keycloakUser.Id);

                await transaction.CommitAsync(token);
            }
            catch
            {
                await transaction.RollbackAsync(token);
                throw;
            }

            await _publish.Publish(new PortfolioUserApproved
            {
                UserName = dbUser.UserName,
                ExternalId = Guid.Parse(keycloakUser.Id),
                InternalId = dbUser.Id,
                ApprovedOn = _timeProvider.Now
            }, token);
        }
        
        private async Task<UserRepresentation> FetchKeycloakUserByUniqueUserName(string userName)
        {
            var keycloakSearchResult = await _userClient.GetUsersByRealm(_config.Realm, 
                exact: "true",
                username: userName);
            
            if (!keycloakSearchResult.Any())
            {
                throw new PortfolioUserNotFoundException("User not found");
            }

            return keycloakSearchResult.First();
        }
    }
}
