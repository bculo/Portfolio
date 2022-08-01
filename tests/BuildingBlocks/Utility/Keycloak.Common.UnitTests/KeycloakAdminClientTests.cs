using Auth0.Abstract.Models;
using AutoFixture;
using FluentAssertions;
using Keycloak.Common.Clients;
using Keycloak.Common.Models.Response.Users;
using Keycloak.Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using System.Net;

namespace Keycloak.Common.UnitTests
{
    public class KeycloakAdminClientTests
    {
        private const string VALID_REALM = "PortfolioRealm";
        private const string VALID_ACCESS_TOKEN = "abc-sir";
        private const string VALID_USER_ID = "e246ce6c-30de-4b82-ba92-cafb31bdc9a0";

        private readonly Fixture _fixture = new Fixture();

        private const string ADMIN_API_POINT = "http://localhost:8080/auth/admin/realms/";
        private readonly IOptions<KeycloakAdminApiOptions> _adminApiOptions;
        private readonly Mock<IHttpClientFactory> _httpAdminApiClientFactory = new Mock<IHttpClientFactory>();
        private readonly ILogger<KeycloakAdminClient> _loggerAdminApi = new Mock<ILogger<KeycloakAdminClient>>().Object;

        private const string OWNER_AUTHORIZATION_API_POINT = "http://localhost:8080/auth/realms/master/";
        private readonly IOptions<KeycloakOwnerCredentialFlowOptions> _ownerApiOptions;
        private readonly Mock<IHttpClientFactory> _httpOwnerClientFactory = new Mock<IHttpClientFactory>();
        private readonly ILogger<KeycloakOwnerCredentialFlowClient> _loggerOwner = new Mock<ILogger<KeycloakOwnerCredentialFlowClient>>().Object;

        public KeycloakAdminClientTests()
        {
            _adminApiOptions = Microsoft.Extensions.Options.Options.Create(new KeycloakAdminApiOptions
            {
                AdminApiEndpointBase = ADMIN_API_POINT,
            });

            _ownerApiOptions = Microsoft.Extensions.Options.Options.Create(new KeycloakOwnerCredentialFlowOptions
            {
                AuthorizationServerUrl = OWNER_AUTHORIZATION_API_POINT
            });
        }

        [Fact]
        public async Task GetUsers_ShouldReturnUserList_WhenValidRealmAndAccessTokenProvided()
        {
            //Arrange
            string realm = VALID_REALM;
            string token = await GetAccessToken();
            var adminClient = CreateAdminClientUsers(realm, token);

            //Act
            var response = await adminClient.GetUsers(realm, token);

            //Assert
            response.Should().NotBeNull();
            response[0].UserId.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetUsers_ShouldThrowArgumentNullException_WhenAnNullParameterProvided()
        {
            //Arrange
            string realm = null;
            string token = await GetAccessToken();
            var adminClient = CreateAdminClientUsers(realm, token);

            //Act
            await Assert.ThrowsAsync<ArgumentNullException>(() => adminClient.GetUsers(realm, token));
        }

        [Fact]
        public async Task GetUsers_ShouldReturnNull_WhenInvalidRealmProvided()
        {
            //Arrange
            string realm = "invalid-realm";
            string token = await GetAccessToken();
            var adminClient = CreateAdminClientUsers(realm, token);

            //Act
            var response = await adminClient.GetUsers(realm, token);

            //Assert
            response.Should().BeNull();
        }

        [Fact]
        public async Task GetUserByid_ShouldReturnInstance_WhenValidDataProvided()
        {
            //Arrange
            string realm = VALID_REALM;
            string token = await GetAccessToken();
            string userId = VALID_USER_ID;
            var adminClient = CreateAdminClientForSingleUser(realm, token, userId);

            //Act
            var response = await adminClient.GetUserById(realm, token, Guid.Parse(userId));

            //Assert
            response.Should().NotBeNull();
            response.UserId.Should().Be(userId);
        }

        [Fact]
        public async Task GetUserByid_ShouldReturnInstance_WhenInvalidDataProvided()
        {
            //Arrange
            string realm = "invalid-realm";
            string token = await GetAccessToken();
            string userId = VALID_USER_ID;
            var adminClient = CreateAdminClientForSingleUser(realm, token, userId);

            //Act
            var response = await adminClient.GetUserById(realm, token, Guid.Parse(userId));

            //Assert
            response.Should().BeNull();
        }

        [Fact]
        public async Task GetUserByid_ShouldThrowArgumentNullException_WhenNullParameterProvided()
        {
            //Arrange
            string realm = null;
            string token = await GetAccessToken();
            string userId = VALID_USER_ID;
            var adminClient = CreateAdminClientForSingleUser(realm, token, userId);

            //Act
            await Assert.ThrowsAsync<ArgumentNullException>(() => adminClient.GetUserById(realm, token, Guid.Parse(userId)));
        }

        [Fact]
        public async Task GetUserByid_ShouldReturnNull_WhenInvalidUserIdentifierProvided()
        {
            //Arrange
            string realm = VALID_REALM;
            string token = await GetAccessToken();
            string userId = Guid.NewGuid().ToString();
            var adminClient = CreateAdminClientForSingleUser(realm, token, userId);

            //Act
            var response = await adminClient.GetUserById(realm, token, Guid.Parse(userId));

            //Assert
            response.Should().BeNull();
        }

        [Fact]
        public async Task ImportRealm_ShouldReturnTrue_WhenValidImportProvided()
        {
            string realm = VALID_REALM;
            var accessToken = await GetAccessToken();
            var client = CreateClient(realm, accessToken, ImportRealmValidScenarion);

            var response = await client.ImportRealm(Json(), accessToken);

        }

        private (bool, string) ImportRealmValidScenarion()
        {
            return (true, "{}");
        }

        /// <summary>
        /// Mocked client for Users endpoint
        /// </summary>
        /// <param name="realm"></param>
        /// <param name="token"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        private KeycloakAdminClient CreateAdminClientUsers(string realm, string token)
        {
            var mockHandler = new MockHttpMessageHandler();
            var handled = false;

            if(realm == VALID_REALM && token == VALID_ACCESS_TOKEN)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond("application/json", JsonConvert.SerializeObject(_fixture.CreateMany<UserResponse>(5)));
                handled = true;
            }

            if(realm != VALID_REALM && token == VALID_ACCESS_TOKEN)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.NotFound, "application/json", "'error': 'Realm not found.'");
                handled = true;
            }

            if (realm == VALID_REALM && token != VALID_ACCESS_TOKEN)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.Unauthorized, "application/json", "'error': 'HTTP 401 Unauthorized'");
                handled = true;
            }

            if(!handled)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.BadRequest);
                handled = true;
            }

            _httpAdminApiClientFactory.Setup(i => i.CreateClient(It.IsAny<string>()))
                .Returns(mockHandler.ToHttpClient());

            return new KeycloakAdminClient(_httpAdminApiClientFactory.Object, _adminApiOptions, _loggerAdminApi);
        }

        private async Task<string> GetAccessToken()
        {
            var mockHandler = new MockHttpMessageHandler();
            var responseInstance = _fixture.Create<TokenAuthorizationCodeResponse>();
            responseInstance.AccessToken = VALID_ACCESS_TOKEN;
            mockHandler.When("*").Respond(HttpStatusCode.OK, "application/json", JsonConvert.SerializeObject(responseInstance));

            _httpOwnerClientFactory.Setup(i => i.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
            var authClient = new KeycloakOwnerCredentialFlowClient(_httpOwnerClientFactory.Object, _ownerApiOptions, _loggerOwner);

            var response = await authClient.GetToken("admin-cli", "admin", "admin");
            return response.AccessToken;
        }

        private KeycloakAdminClient CreateClient(string realm, string token, Func<(bool valid, string json)> func)
        {
            var handerMocked = false;
            var mockHandler = new MockHttpMessageHandler();

            var (valid, json) = func();

            if (realm == VALID_REALM && token == VALID_ACCESS_TOKEN && valid)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond("application/json", json);
                handerMocked = true;
            }

            //INVALID REALM
            if (realm != VALID_REALM && token == VALID_ACCESS_TOKEN && valid)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.NotFound, "application/json", "'error': 'Realm not found.'");
                handerMocked = true;
            }

            //INVALID ACCESS TOKEN
            if (realm == VALID_REALM && token != VALID_ACCESS_TOKEN && valid)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.Unauthorized, "application/json", "'error': 'HTTP 401 Unauthorized'");
                handerMocked = true;
            }

            //INVALID DATA
            if (!handerMocked && !valid)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.BadRequest);
            }

            _httpAdminApiClientFactory.Setup(i => i.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient());

            return new KeycloakAdminClient(_httpAdminApiClientFactory.Object, _adminApiOptions, _loggerAdminApi);
        }

        private KeycloakAdminClient CreateClient<T>(string realm, string token, Func<T, (bool valid, string json)> func, T instance)
        {
            var handerMocked = false;
            var mockHandler = new MockHttpMessageHandler();

            var (valid, json) = func(instance);

            if (realm == VALID_REALM && token == VALID_ACCESS_TOKEN && valid)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond("application/json", json);
                handerMocked = true;
            }

            //INVALID REALM
            if (realm != VALID_REALM && token == VALID_ACCESS_TOKEN && valid)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.NotFound, "application/json", "'error': 'Realm not found.'");
                handerMocked = true;
            }

            //INVALID ACCESS TOKEN
            if (realm == VALID_REALM && token != VALID_ACCESS_TOKEN && valid)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.Unauthorized, "application/json", "'error': 'HTTP 401 Unauthorized'");
                handerMocked = true;
            }

            //INVALID DATA
            if (!handerMocked && !valid)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.BadRequest);
            }

            _httpAdminApiClientFactory.Setup(i => i.CreateClient(It.IsAny<string>()))
                .Returns(mockHandler.ToHttpClient());

            return new KeycloakAdminClient(_httpAdminApiClientFactory.Object, _adminApiOptions, _loggerAdminApi);
        }

        /// <summary>
        /// Mocked client for fetching single user endpoint
        /// </summary>
        /// <param name="realm"></param>
        /// <param name="token"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        private KeycloakAdminClient CreateAdminClientForSingleUser(string realm, string token, string userId)
        {
            var handerMocked = false;
            var mockHandler = new MockHttpMessageHandler();

            //VALID DATA
            if (realm == VALID_REALM && token == VALID_ACCESS_TOKEN && userId == VALID_USER_ID)
            {
                var validResponse = _fixture.Create<UserResponse>();
                validResponse.UserId = Guid.Parse(VALID_USER_ID);
                mockHandler.When(HttpMethod.Get, "*").Respond("application/json", JsonConvert.SerializeObject(validResponse));
                handerMocked = true;
            }

            //INVALID REALM
            if (realm != VALID_REALM && token == VALID_ACCESS_TOKEN && userId == VALID_USER_ID)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.NotFound, "application/json", "'error': 'Realm not found.'");
                handerMocked = true;
            }

            //INVALID ACCESS TOKEN
            if (realm == VALID_REALM && token != VALID_ACCESS_TOKEN && userId == VALID_USER_ID)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.Unauthorized, "application/json", "'error': 'HTTP 401 Unauthorized'");
                handerMocked = true;
            }

            //INVALID USER ID
            if (realm == VALID_REALM && token == VALID_ACCESS_TOKEN && userId != VALID_USER_ID)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.NotFound, "application/json", "'error': 'User not found.'");
                handerMocked = true;
            }

            //INVALID DATA
            if (!handerMocked)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.BadRequest);
            }

            _httpAdminApiClientFactory.Setup(i => i.CreateClient(It.IsAny<string>()))
                .Returns(mockHandler.ToHttpClient());

            return new KeycloakAdminClient(_httpAdminApiClientFactory.Object, _adminApiOptions, _loggerAdminApi);
        }

        private string Json()
        {
            return @"{
  'id': 'PortfolioTestRealm',
  'realm': 'PortfolioTestRealm',
  'notBefore': 1657889614,
  'defaultSignatureAlgorithm': 'RS256',
  'revokeRefreshToken': false,
  'refreshTokenMaxReuse': 0,
  'accessTokenLifespan': 300,
  'accessTokenLifespanForImplicitFlow': 900,
  'ssoSessionIdleTimeout': 1800,
  'ssoSessionMaxLifespan': 36000,
  'ssoSessionIdleTimeoutRememberMe': 0,
  'ssoSessionMaxLifespanRememberMe': 0,
  'offlineSessionIdleTimeout': 2592000,
  'offlineSessionMaxLifespanEnabled': false,
  'offlineSessionMaxLifespan': 5184000,
  'clientSessionIdleTimeout': 0,
  'clientSessionMaxLifespan': 0,
  'clientOfflineSessionIdleTimeout': 0,
  'clientOfflineSessionMaxLifespan': 0,
  'accessCodeLifespan': 60,
  'accessCodeLifespanUserAction': 300,
  'accessCodeLifespanLogin': 1800,
  'actionTokenGeneratedByAdminLifespan': 43200,
  'actionTokenGeneratedByUserLifespan': 300,
  'oauth2DeviceCodeLifespan': 600,
  'oauth2DevicePollingInterval': 5,
  'enabled': true,
  'sslRequired': 'external',
  'registrationAllowed': true,
  'registrationEmailAsUsername': false,
  'rememberMe': false,
  'verifyEmail': false,
  'loginWithEmailAllowed': true,
  'duplicateEmailsAllowed': false,
  'resetPasswordAllowed': false,
  'editUsernameAllowed': false,
  'bruteForceProtected': false,
  'permanentLockout': false,
  'maxFailureWaitSeconds': 900,
  'minimumQuickLoginWaitSeconds': 60,
  'waitIncrementSeconds': 60,
  'quickLoginCheckMilliSeconds': 1000,
  'maxDeltaTimeSeconds': 43200,
  'failureFactor': 30,
  'roles': {
    'realm': [
      {
        'id': 'b1b7b394-0bf1-4b06-9138-f100670558d2',
        'name': 'Application',
        'composite': false,
        'clientRole': false,
        'containerId': 'PortfolioRealm',
        'attributes': {}
      },
      {
        'id': '5bb336c6-1f40-4207-a943-da2871f6e6d1',
        'name': 'uma_authorization',
        'description': '${role_uma_authorization}',
        'composite': false,
        'clientRole': false,
        'containerId': 'PortfolioRealm',
        'attributes': {}
      },
      {
        'id': '3246486e-ae0b-4990-b6ba-3570a99bcd27',
        'name': 'Moderator',
        'composite': false,
        'clientRole': false,
        'containerId': 'PortfolioRealm',
        'attributes': {}
      },
      {
        'id': '2c222dec-9664-4df4-835e-9c6fbd4b100f',
        'name': 'default-roles-portfoliorealm',
        'description': '${role_default-roles}',
        'composite': true,
        'composites': {
          'realm': [
            'User',
            'offline_access',
            'uma_authorization'
          ],
          'client': {
            'Trend.Client': [
              'User'
            ],
            'account': [
              'manage-account',
              'view-profile'
            ]
          }
        },
        'clientRole': false,
        'containerId': 'PortfolioRealm',
        'attributes': {}
      },
      {
        'id': 'ea101722-c674-46f2-83ce-1228495d1c74',
        'name': 'Vip',
        'composite': false,
        'clientRole': false,
        'containerId': 'PortfolioRealm',
        'attributes': {}
      },
      {
        'id': 'c6b354bb-75af-4500-a46e-3cd7fb04badc',
        'name': 'offline_access',
        'description': '${role_offline-access}',
        'composite': false,
        'clientRole': false,
        'containerId': 'PortfolioRealm',
        'attributes': {}
      },
      {
        'id': 'ac51520d-9fad-4030-b68c-6fdf3206a4e0',
        'name': 'Admin',
        'composite': false,
        'clientRole': false,
        'containerId': 'PortfolioRealm',
        'attributes': {}
      },
      {
        'id': 'b588f9cb-bbb5-40b1-9d71-455097cac3cb',
        'name': 'User',
        'composite': false,
        'clientRole': false,
        'containerId': 'PortfolioRealm',
        'attributes': {}
      }
    ],
    'client': {
      'realm-management': [
        {
          'id': 'fbfeb166-b93d-4a97-892c-f95dc740c3a1',
          'name': 'manage-identity-providers',
          'description': '${role_manage-identity-providers}',
          'composite': false,
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        },
        {
          'id': '79c41b02-aaec-48a5-a233-628d5ce13453',
          'name': 'query-users',
          'description': '${role_query-users}',
          'composite': false,
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        },
        {
          'id': '22da3943-14ff-4afe-9b88-277864ab807e',
          'name': 'manage-events',
          'description': '${role_manage-events}',
          'composite': false,
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        },
        {
          'id': '998e6902-089c-4e72-8954-0ea70cd9dab9',
          'name': 'realm-admin',
          'description': '${role_realm-admin}',
          'composite': true,
          'composites': {
            'client': {
              'realm-management': [
                'query-users',
                'manage-identity-providers',
                'manage-events',
                'view-users',
                'view-clients',
                'view-authorization',
                'view-identity-providers',
                'query-groups',
                'manage-users',
                'impersonation',
                'manage-authorization',
                'create-client',
                'query-realms',
                'query-clients',
                'manage-realm',
                'manage-clients',
                'view-realm',
                'view-events'
              ]
            }
          },
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        },
        {
          'id': 'd7a596f7-fb1a-4e85-a2a2-33433d115d79',
          'name': 'view-users',
          'description': '${role_view-users}',
          'composite': true,
          'composites': {
            'client': {
              'realm-management': [
                'query-users',
                'query-groups'
              ]
            }
          },
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        },
        {
          'id': '9cd68cf9-9eba-492e-bb5c-b2328a0b78fc',
          'name': 'view-clients',
          'description': '${role_view-clients}',
          'composite': true,
          'composites': {
            'client': {
              'realm-management': [
                'query-clients'
              ]
            }
          },
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        },
        {
          'id': '22466898-e87f-4c45-860b-707989086de6',
          'name': 'view-authorization',
          'description': '${role_view-authorization}',
          'composite': false,
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        },
        {
          'id': 'cb653e16-b300-419f-b54d-ba5c99e7434d',
          'name': 'query-groups',
          'description': '${role_query-groups}',
          'composite': false,
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        },
        {
          'id': '2a7da7c3-59cc-4ab5-8f90-92f3dbcd3454',
          'name': 'view-identity-providers',
          'description': '${role_view-identity-providers}',
          'composite': false,
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        },
        {
          'id': '0a6abdd9-2176-485d-a2df-1ce7b913521e',
          'name': 'manage-users',
          'description': '${role_manage-users}',
          'composite': false,
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        },
        {
          'id': 'd016cc73-2e99-4fae-bdba-15eb0a27ecb1',
          'name': 'impersonation',
          'description': '${role_impersonation}',
          'composite': false,
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        },
        {
          'id': '403afc75-a226-40dc-ac0a-28f6b95c6235',
          'name': 'manage-authorization',
          'description': '${role_manage-authorization}',
          'composite': false,
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        },
        {
          'id': 'd453011a-27b0-46f4-a264-24ad7e1dfb4f',
          'name': 'create-client',
          'description': '${role_create-client}',
          'composite': false,
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        },
        {
          'id': '70732ba6-d7cf-4c7c-88a6-6c2e2f626eec',
          'name': 'query-realms',
          'description': '${role_query-realms}',
          'composite': false,
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        },
        {
          'id': 'efbc41cf-6c9a-4cdf-a664-d9cb199b864b',
          'name': 'query-clients',
          'description': '${role_query-clients}',
          'composite': false,
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        },
        {
          'id': 'a974c41d-e4b5-4ff5-bb73-d70887600c9d',
          'name': 'manage-clients',
          'description': '${role_manage-clients}',
          'composite': false,
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        },
        {
          'id': '13bcfdce-38b8-4c0f-b036-38393745e01c',
          'name': 'manage-realm',
          'description': '${role_manage-realm}',
          'composite': false,
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        },
        {
          'id': 'f2f78d04-6a5b-455a-8a80-b915106f7d9c',
          'name': 'view-realm',
          'description': '${role_view-realm}',
          'composite': false,
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        },
        {
          'id': '8dbd616c-4fd3-4201-b52a-34166b3d9320',
          'name': 'view-events',
          'description': '${role_view-events}',
          'composite': false,
          'clientRole': true,
          'containerId': 'c0865956-09ee-447b-ab20-faccb97d771b',
          'attributes': {}
        }
      ],
      'Trend.API': [],
      'Test.API': [],
      'Trend.Client': [
        {
          'id': 'a3911cd3-791d-487a-a196-4e4b79f9fab4',
          'name': 'Admin',
          'composite': false,
          'clientRole': true,
          'containerId': '6ce6dc6f-1297-4aaa-9b22-f6d98058ebc6',
          'attributes': {}
        },
        {
          'id': '678f9b70-90ba-4a9c-8476-057893de0b67',
          'name': 'User',
          'composite': false,
          'clientRole': true,
          'containerId': '6ce6dc6f-1297-4aaa-9b22-f6d98058ebc6',
          'attributes': {}
        }
      ],
      'security-admin-console': [],
      'admin-cli': [],
      'account-console': [],
      'broker': [
        {
          'id': '7a009434-3726-4049-995b-f715ff81bee7',
          'name': 'read-token',
          'description': '${role_read-token}',
          'composite': false,
          'clientRole': true,
          'containerId': '9ec168ab-11f4-4d2b-bfb9-8fb45f4bd1a3',
          'attributes': {}
        }
      ],
      'account': [
        {
          'id': 'dc3d0753-e18a-459b-a171-ec52622fab7e',
          'name': 'manage-consent',
          'description': '${role_manage-consent}',
          'composite': true,
          'composites': {
            'client': {
              'account': [
                'view-consent'
              ]
            }
          },
          'clientRole': true,
          'containerId': '1636276f-b6d8-4730-93cf-c923784eb0aa',
          'attributes': {}
        },
        {
          'id': '4568f15c-899c-439f-a7d6-bcb4bb388c2c',
          'name': 'view-consent',
          'description': '${role_view-consent}',
          'composite': false,
          'clientRole': true,
          'containerId': '1636276f-b6d8-4730-93cf-c923784eb0aa',
          'attributes': {}
        },
        {
          'id': '9236c909-e32c-4546-a8e2-e8c8713c4e64',
          'name': 'delete-account',
          'description': '${role_delete-account}',
          'composite': false,
          'clientRole': true,
          'containerId': '1636276f-b6d8-4730-93cf-c923784eb0aa',
          'attributes': {}
        },
        {
          'id': '58d074ab-cc8f-46e2-bee1-5bbacc694439',
          'name': 'manage-account',
          'description': '${role_manage-account}',
          'composite': true,
          'composites': {
            'client': {
              'account': [
                'manage-account-links'
              ]
            }
          },
          'clientRole': true,
          'containerId': '1636276f-b6d8-4730-93cf-c923784eb0aa',
          'attributes': {}
        },
        {
          'id': '00d414e3-61e3-41ad-84be-0ee8ea272eec',
          'name': 'view-profile',
          'description': '${role_view-profile}',
          'composite': false,
          'clientRole': true,
          'containerId': '1636276f-b6d8-4730-93cf-c923784eb0aa',
          'attributes': {}
        },
        {
          'id': '3ab6e480-e55f-40a2-b748-890533d0b8b6',
          'name': 'manage-account-links',
          'description': '${role_manage-account-links}',
          'composite': false,
          'clientRole': true,
          'containerId': '1636276f-b6d8-4730-93cf-c923784eb0aa',
          'attributes': {}
        },
        {
          'id': '67bc7e2e-804e-40a1-8e49-bc5364239144',
          'name': 'view-applications',
          'description': '${role_view-applications}',
          'composite': false,
          'clientRole': true,
          'containerId': '1636276f-b6d8-4730-93cf-c923784eb0aa',
          'attributes': {}
        }
      ]
    }
  },
  'groups': [],
  'defaultRole': {
    'id': '2c222dec-9664-4df4-835e-9c6fbd4b100f',
    'name': 'default-roles-portfoliorealm',
    'description': '${role_default-roles}',
    'composite': true,
    'clientRole': false,
    'containerId': 'PortfolioRealm'
  },
  'requiredCredentials': [
    'password'
  ],
  'otpPolicyType': 'totp',
  'otpPolicyAlgorithm': 'HmacSHA1',
  'otpPolicyInitialCounter': 0,
  'otpPolicyDigits': 6,
  'otpPolicyLookAheadWindow': 1,
  'otpPolicyPeriod': 30,
  'otpSupportedApplications': [
    'FreeOTP',
    'Google Authenticator'
  ],
  'webAuthnPolicyRpEntityName': 'keycloak',
  'webAuthnPolicySignatureAlgorithms': [
    'ES256'
  ],
  'webAuthnPolicyRpId': '',
  'webAuthnPolicyAttestationConveyancePreference': 'not specified',
  'webAuthnPolicyAuthenticatorAttachment': 'not specified',
  'webAuthnPolicyRequireResidentKey': 'not specified',
  'webAuthnPolicyUserVerificationRequirement': 'not specified',
  'webAuthnPolicyCreateTimeout': 0,
  'webAuthnPolicyAvoidSameAuthenticatorRegister': false,
  'webAuthnPolicyAcceptableAaguids': [],
  'webAuthnPolicyPasswordlessRpEntityName': 'keycloak',
  'webAuthnPolicyPasswordlessSignatureAlgorithms': [
    'ES256'
  ],
  'webAuthnPolicyPasswordlessRpId': '',
  'webAuthnPolicyPasswordlessAttestationConveyancePreference': 'not specified',
  'webAuthnPolicyPasswordlessAuthenticatorAttachment': 'not specified',
  'webAuthnPolicyPasswordlessRequireResidentKey': 'not specified',
  'webAuthnPolicyPasswordlessUserVerificationRequirement': 'not specified',
  'webAuthnPolicyPasswordlessCreateTimeout': 0,
  'webAuthnPolicyPasswordlessAvoidSameAuthenticatorRegister': false,
  'webAuthnPolicyPasswordlessAcceptableAaguids': [],
  'users': [
    {
      'id': '9241fddc-bc15-4a2d-9698-afd759e8b6a2',
      'createdTimestamp': 1658322059500,
      'username': 'service-account-trend.api',
      'enabled': true,
      'totp': false,
      'emailVerified': false,
      'serviceAccountClientId': 'Trend.API',
      'disableableCredentialTypes': [],
      'requiredActions': [
        'CONFIGURE_TOTP'
      ],
      'realmRoles': [
        'Application',
        'default-roles-portfoliorealm'
      ],
      'notBefore': 0,
      'groups': []
    }
  ],
  'scopeMappings': [
    {
      'client': 'Trend.API',
      'roles': [
        'Application'
      ]
    },
    {
      'clientScope': 'offline_access',
      'roles': [
        'offline_access'
      ]
    }
  ],
  'clientScopeMappings': {
    'account': [
      {
        'client': 'account-console',
        'roles': [
          'manage-account'
        ]
      }
    ]
  },
  'clients': [
    {
      'id': '1636276f-b6d8-4730-93cf-c923784eb0aa',
      'clientId': 'account',
      'name': '${client_account}',
      'rootUrl': '${authBaseUrl}',
      'baseUrl': '/realms/PortfolioRealm/account/',
      'surrogateAuthRequired': false,
      'enabled': true,
      'alwaysDisplayInConsole': false,
      'clientAuthenticatorType': 'client-secret',
      'redirectUris': [
        '/realms/PortfolioRealm/account/*'
      ],
      'webOrigins': [],
      'notBefore': 0,
      'bearerOnly': false,
      'consentRequired': false,
      'standardFlowEnabled': true,
      'implicitFlowEnabled': false,
      'directAccessGrantsEnabled': false,
      'serviceAccountsEnabled': false,
      'publicClient': true,
      'frontchannelLogout': false,
      'protocol': 'openid-connect',
      'attributes': {},
      'authenticationFlowBindingOverrides': {},
      'fullScopeAllowed': false,
      'nodeReRegistrationTimeout': 0,
      'defaultClientScopes': [
        'web-origins',
        'roles',
        'profile',
        'email'
      ],
      'optionalClientScopes': [
        'address',
        'phone',
        'offline_access',
        'microprofile-jwt'
      ]
    },
    {
      'id': 'b126a82b-7c88-44f5-a5d3-641bb337adab',
      'clientId': 'account-console',
      'name': '${client_account-console}',
      'rootUrl': '${authBaseUrl}',
      'baseUrl': '/realms/PortfolioRealm/account/',
      'surrogateAuthRequired': false,
      'enabled': true,
      'alwaysDisplayInConsole': false,
      'clientAuthenticatorType': 'client-secret',
      'redirectUris': [
        '/realms/PortfolioRealm/account/*'
      ],
      'webOrigins': [],
      'notBefore': 0,
      'bearerOnly': false,
      'consentRequired': false,
      'standardFlowEnabled': true,
      'implicitFlowEnabled': false,
      'directAccessGrantsEnabled': false,
      'serviceAccountsEnabled': false,
      'publicClient': true,
      'frontchannelLogout': false,
      'protocol': 'openid-connect',
      'attributes': {
        'pkce.code.challenge.method': 'S256'
      },
      'authenticationFlowBindingOverrides': {},
      'fullScopeAllowed': false,
      'nodeReRegistrationTimeout': 0,
      'protocolMappers': [
        {
          'id': 'f04d6f41-d5c6-421e-9622-269b01327f06',
          'name': 'audience resolve',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-audience-resolve-mapper',
          'consentRequired': false,
          'config': {}
        }
      ],
      'defaultClientScopes': [
        'web-origins',
        'roles',
        'profile',
        'email'
      ],
      'optionalClientScopes': [
        'address',
        'phone',
        'offline_access',
        'microprofile-jwt'
      ]
    },
    {
      'id': '6f37c0a9-30d8-4f6a-b1be-60fbc4fc1d36',
      'clientId': 'admin-cli',
      'name': '${client_admin-cli}',
      'surrogateAuthRequired': false,
      'enabled': true,
      'alwaysDisplayInConsole': false,
      'clientAuthenticatorType': 'client-secret',
      'redirectUris': [],
      'webOrigins': [],
      'notBefore': 0,
      'bearerOnly': false,
      'consentRequired': false,
      'standardFlowEnabled': false,
      'implicitFlowEnabled': false,
      'directAccessGrantsEnabled': true,
      'serviceAccountsEnabled': false,
      'publicClient': true,
      'frontchannelLogout': false,
      'protocol': 'openid-connect',
      'attributes': {},
      'authenticationFlowBindingOverrides': {},
      'fullScopeAllowed': false,
      'nodeReRegistrationTimeout': 0,
      'defaultClientScopes': [
        'web-origins',
        'roles',
        'profile',
        'email'
      ],
      'optionalClientScopes': [
        'address',
        'phone',
        'offline_access',
        'microprofile-jwt'
      ]
    },
    {
      'id': '9ec168ab-11f4-4d2b-bfb9-8fb45f4bd1a3',
      'clientId': 'broker',
      'name': '${client_broker}',
      'surrogateAuthRequired': false,
      'enabled': true,
      'alwaysDisplayInConsole': false,
      'clientAuthenticatorType': 'client-secret',
      'redirectUris': [],
      'webOrigins': [],
      'notBefore': 0,
      'bearerOnly': true,
      'consentRequired': false,
      'standardFlowEnabled': true,
      'implicitFlowEnabled': false,
      'directAccessGrantsEnabled': false,
      'serviceAccountsEnabled': false,
      'publicClient': false,
      'frontchannelLogout': false,
      'protocol': 'openid-connect',
      'attributes': {},
      'authenticationFlowBindingOverrides': {},
      'fullScopeAllowed': false,
      'nodeReRegistrationTimeout': 0,
      'defaultClientScopes': [
        'web-origins',
        'roles',
        'profile',
        'email'
      ],
      'optionalClientScopes': [
        'address',
        'phone',
        'offline_access',
        'microprofile-jwt'
      ]
    },
    {
      'id': 'c0865956-09ee-447b-ab20-faccb97d771b',
      'clientId': 'realm-management',
      'name': '${client_realm-management}',
      'surrogateAuthRequired': false,
      'enabled': true,
      'alwaysDisplayInConsole': false,
      'clientAuthenticatorType': 'client-secret',
      'redirectUris': [],
      'webOrigins': [],
      'notBefore': 0,
      'bearerOnly': true,
      'consentRequired': false,
      'standardFlowEnabled': true,
      'implicitFlowEnabled': false,
      'directAccessGrantsEnabled': false,
      'serviceAccountsEnabled': false,
      'publicClient': false,
      'frontchannelLogout': false,
      'protocol': 'openid-connect',
      'attributes': {},
      'authenticationFlowBindingOverrides': {},
      'fullScopeAllowed': false,
      'nodeReRegistrationTimeout': 0,
      'defaultClientScopes': [
        'web-origins',
        'roles',
        'profile',
        'email'
      ],
      'optionalClientScopes': [
        'address',
        'phone',
        'offline_access',
        'microprofile-jwt'
      ]
    },
    {
      'id': '471e39c8-ca4e-4fcc-bf71-530962160f50',
      'clientId': 'security-admin-console',
      'name': '${client_security-admin-console}',
      'rootUrl': '${authAdminUrl}',
      'baseUrl': '/admin/PortfolioRealm/console/',
      'surrogateAuthRequired': false,
      'enabled': true,
      'alwaysDisplayInConsole': false,
      'clientAuthenticatorType': 'client-secret',
      'redirectUris': [
        '/admin/PortfolioRealm/console/*'
      ],
      'webOrigins': [
        '+'
      ],
      'notBefore': 0,
      'bearerOnly': false,
      'consentRequired': false,
      'standardFlowEnabled': true,
      'implicitFlowEnabled': false,
      'directAccessGrantsEnabled': false,
      'serviceAccountsEnabled': false,
      'publicClient': true,
      'frontchannelLogout': false,
      'protocol': 'openid-connect',
      'attributes': {
        'pkce.code.challenge.method': 'S256'
      },
      'authenticationFlowBindingOverrides': {},
      'fullScopeAllowed': false,
      'nodeReRegistrationTimeout': 0,
      'protocolMappers': [
        {
          'id': 'd7d3b092-8c60-4763-83a5-f5ee903500c5',
          'name': 'locale',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-attribute-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'locale',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'locale',
            'jsonType.label': 'String'
          }
        }
      ],
      'defaultClientScopes': [
        'web-origins',
        'roles',
        'profile',
        'email'
      ],
      'optionalClientScopes': [
        'address',
        'phone',
        'offline_access',
        'microprofile-jwt'
      ]
    },
    {
      'id': '2dc57e6c-206f-4c9b-8bf9-312ac7ed7237',
      'clientId': 'Test.API',
      'surrogateAuthRequired': false,
      'enabled': true,
      'alwaysDisplayInConsole': false,
      'clientAuthenticatorType': 'client-secret',
      'redirectUris': [],
      'webOrigins': [],
      'notBefore': 0,
      'bearerOnly': false,
      'consentRequired': false,
      'standardFlowEnabled': true,
      'implicitFlowEnabled': false,
      'directAccessGrantsEnabled': true,
      'serviceAccountsEnabled': false,
      'publicClient': true,
      'frontchannelLogout': false,
      'protocol': 'openid-connect',
      'attributes': {
        'backchannel.logout.session.required': 'true',
        'backchannel.logout.revoke.offline.tokens': 'false'
      },
      'authenticationFlowBindingOverrides': {},
      'fullScopeAllowed': true,
      'nodeReRegistrationTimeout': -1,
      'defaultClientScopes': [
        'web-origins',
        'roles',
        'profile',
        'email'
      ],
      'optionalClientScopes': [
        'address',
        'phone',
        'offline_access',
        'microprofile-jwt'
      ]
    },
    {
      'id': '5eec38c1-424e-46ee-91ca-a15ff692ca31',
      'clientId': 'Trend.API',
      'surrogateAuthRequired': false,
      'enabled': true,
      'alwaysDisplayInConsole': false,
      'clientAuthenticatorType': 'client-secret',
      'secret': '**********',
      'redirectUris': [
        'http://localhost:5276/'
      ],
      'webOrigins': [],
      'notBefore': 0,
      'bearerOnly': false,
      'consentRequired': false,
      'standardFlowEnabled': true,
      'implicitFlowEnabled': false,
      'directAccessGrantsEnabled': true,
      'serviceAccountsEnabled': true,
      'publicClient': false,
      'frontchannelLogout': false,
      'protocol': 'openid-connect',
      'attributes': {
        'id.token.as.detached.signature': 'false',
        'saml.assertion.signature': 'false',
        'saml.force.post.binding': 'false',
        'saml.multivalued.roles': 'false',
        'saml.encrypt': 'false',
        'oauth2.device.authorization.grant.enabled': 'false',
        'backchannel.logout.revoke.offline.tokens': 'false',
        'saml.server.signature': 'false',
        'saml.server.signature.keyinfo.ext': 'false',
        'use.refresh.tokens': 'true',
        'exclude.session.state.from.auth.response': 'false',
        'oidc.ciba.grant.enabled': 'false',
        'saml.artifact.binding': 'false',
        'backchannel.logout.session.required': 'true',
        'client_credentials.use_refresh_token': 'false',
        'saml_force_name_id_format': 'false',
        'require.pushed.authorization.requests': 'false',
        'saml.client.signature': 'false',
        'tls.client.certificate.bound.access.tokens': 'false',
        'saml.authnstatement': 'false',
        'display.on.consent.screen': 'false',
        'saml.onetimeuse.condition': 'false'
      },
      'authenticationFlowBindingOverrides': {},
      'fullScopeAllowed': false,
      'nodeReRegistrationTimeout': -1,
      'protocolMappers': [
        {
          'id': 'c3209e74-3b17-44e6-87e7-ea1c63a08c2a',
          'name': 'Client Host',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usersessionmodel-note-mapper',
          'consentRequired': false,
          'config': {
            'user.session.note': 'clientHost',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'clientHost',
            'jsonType.label': 'String'
          }
        },
        {
          'id': 'baf48d90-f6bb-4cb2-8e49-13aa1954fb82',
          'name': 'Client ID',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usersessionmodel-note-mapper',
          'consentRequired': false,
          'config': {
            'user.session.note': 'clientId',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'clientId',
            'jsonType.label': 'String'
          }
        },
        {
          'id': '2f95a15b-fd52-4527-9f6d-3d3b84741a3b',
          'name': 'Client IP Address',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usersessionmodel-note-mapper',
          'consentRequired': false,
          'config': {
            'user.session.note': 'clientAddress',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'clientAddress',
            'jsonType.label': 'String'
          }
        }
      ],
      'defaultClientScopes': [
        'web-origins',
        'roles',
        'profile',
        'email'
      ],
      'optionalClientScopes': [
        'address',
        'phone',
        'offline_access',
        'microprofile-jwt'
      ]
    },
    {
      'id': '6ce6dc6f-1297-4aaa-9b22-f6d98058ebc6',
      'clientId': 'Trend.Client',
      'surrogateAuthRequired': false,
      'enabled': true,
      'alwaysDisplayInConsole': false,
      'clientAuthenticatorType': 'client-secret',
      'redirectUris': [
        'http://localhost:4200/*'
      ],
      'webOrigins': [
        'http://localhost:4200'
      ],
      'notBefore': 0,
      'bearerOnly': false,
      'consentRequired': false,
      'standardFlowEnabled': true,
      'implicitFlowEnabled': false,
      'directAccessGrantsEnabled': true,
      'serviceAccountsEnabled': false,
      'publicClient': true,
      'frontchannelLogout': false,
      'protocol': 'openid-connect',
      'attributes': {
        'id.token.as.detached.signature': 'false',
        'saml.assertion.signature': 'false',
        'saml.force.post.binding': 'false',
        'saml.multivalued.roles': 'false',
        'saml.encrypt': 'false',
        'oauth2.device.authorization.grant.enabled': 'false',
        'backchannel.logout.revoke.offline.tokens': 'false',
        'saml.server.signature': 'false',
        'saml.server.signature.keyinfo.ext': 'false',
        'use.refresh.tokens': 'true',
        'exclude.session.state.from.auth.response': 'false',
        'oidc.ciba.grant.enabled': 'false',
        'saml.artifact.binding': 'false',
        'backchannel.logout.session.required': 'true',
        'client_credentials.use_refresh_token': 'false',
        'saml_force_name_id_format': 'false',
        'require.pushed.authorization.requests': 'false',
        'saml.client.signature': 'false',
        'tls.client.certificate.bound.access.tokens': 'false',
        'saml.authnstatement': 'false',
        'display.on.consent.screen': 'false',
        'saml.onetimeuse.condition': 'false'
      },
      'authenticationFlowBindingOverrides': {},
      'fullScopeAllowed': true,
      'nodeReRegistrationTimeout': -1,
      'defaultClientScopes': [
        'web-origins',
        'roles',
        'profile',
        'email'
      ],
      'optionalClientScopes': [
        'address',
        'phone',
        'offline_access',
        'microprofile-jwt'
      ]
    }
  ],
  'clientScopes': [
    {
      'id': 'cfe2a60d-546d-4b42-b721-0f2781f5d505',
      'name': 'microprofile-jwt',
      'description': 'Microprofile - JWT built-in scope',
      'protocol': 'openid-connect',
      'attributes': {
        'include.in.token.scope': 'true',
        'display.on.consent.screen': 'false'
      },
      'protocolMappers': [
        {
          'id': '7bfba1c5-e1ce-4391-8880-6b837be4c63a',
          'name': 'upn',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-property-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'username',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'upn',
            'jsonType.label': 'String'
          }
        },
        {
          'id': '8941a985-58ff-42b4-879e-8522f9e8988c',
          'name': 'groups',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-realm-role-mapper',
          'consentRequired': false,
          'config': {
            'multivalued': 'true',
            'user.attribute': 'foo',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'groups',
            'jsonType.label': 'String'
          }
        }
      ]
    },
    {
      'id': '9cdd619a-33bf-4f85-86b1-b26a6b39251b',
      'name': 'phone',
      'description': 'OpenID Connect built-in scope: phone',
      'protocol': 'openid-connect',
      'attributes': {
        'include.in.token.scope': 'true',
        'display.on.consent.screen': 'true',
        'consent.screen.text': '${phoneScopeConsentText}'
      },
      'protocolMappers': [
        {
          'id': 'a8527fba-6654-4289-ac14-1d2730f3969f',
          'name': 'phone number verified',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-attribute-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'phoneNumberVerified',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'phone_number_verified',
            'jsonType.label': 'boolean'
          }
        },
        {
          'id': 'c77938ce-e229-4818-84da-1683d79e9e63',
          'name': 'phone number',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-attribute-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'phoneNumber',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'phone_number',
            'jsonType.label': 'String'
          }
        }
      ]
    },
    {
      'id': '1df67ff4-a60c-45ca-a5b6-e757ed7a2f85',
      'name': 'address',
      'description': 'OpenID Connect built-in scope: address',
      'protocol': 'openid-connect',
      'attributes': {
        'include.in.token.scope': 'true',
        'display.on.consent.screen': 'true',
        'consent.screen.text': '${addressScopeConsentText}'
      },
      'protocolMappers': [
        {
          'id': '53b9e2bf-05c7-4e20-8eed-c5be83bfe792',
          'name': 'address',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-address-mapper',
          'consentRequired': false,
          'config': {
            'user.attribute.formatted': 'formatted',
            'user.attribute.country': 'country',
            'user.attribute.postal_code': 'postal_code',
            'userinfo.token.claim': 'true',
            'user.attribute.street': 'street',
            'id.token.claim': 'true',
            'user.attribute.region': 'region',
            'access.token.claim': 'true',
            'user.attribute.locality': 'locality'
          }
        }
      ]
    },
    {
      'id': '71a37058-7bb3-4eb0-a88c-7cfe08994889',
      'name': 'offline_access',
      'description': 'OpenID Connect built-in scope: offline_access',
      'protocol': 'openid-connect',
      'attributes': {
        'consent.screen.text': '${offlineAccessScopeConsentText}',
        'display.on.consent.screen': 'true'
      }
    },
    {
      'id': '0db53450-29d1-45fd-abd1-c26cd6278974',
      'name': 'email',
      'description': 'OpenID Connect built-in scope: email',
      'protocol': 'openid-connect',
      'attributes': {
        'include.in.token.scope': 'true',
        'display.on.consent.screen': 'true',
        'consent.screen.text': '${emailScopeConsentText}'
      },
      'protocolMappers': [
        {
          'id': 'f26c0651-042e-4a67-99ad-53632c4997bf',
          'name': 'email',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-property-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'email',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'email',
            'jsonType.label': 'String'
          }
        },
        {
          'id': '28207357-2880-4cc6-8ef4-d0fb80408ad9',
          'name': 'email verified',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-property-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'emailVerified',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'email_verified',
            'jsonType.label': 'boolean'
          }
        }
      ]
    },
    {
      'id': '0f38ad4e-5fb8-4f88-aa6f-5894f3e1713b',
      'name': 'role_list',
      'description': 'SAML role list',
      'protocol': 'saml',
      'attributes': {
        'consent.screen.text': '${samlRoleListScopeConsentText}',
        'display.on.consent.screen': 'true'
      },
      'protocolMappers': [
        {
          'id': 'be80a07e-418d-43c6-badf-5568c4a13418',
          'name': 'role list',
          'protocol': 'saml',
          'protocolMapper': 'saml-role-list-mapper',
          'consentRequired': false,
          'config': {
            'single': 'false',
            'attribute.nameformat': 'Basic',
            'attribute.name': 'Role'
          }
        }
      ]
    },
    {
      'id': '210e97b9-ff18-4f82-948c-75905b98b2d3',
      'name': 'roles',
      'description': 'OpenID Connect scope for add user roles to the access token',
      'protocol': 'openid-connect',
      'attributes': {
        'include.in.token.scope': 'false',
        'display.on.consent.screen': 'true',
        'consent.screen.text': '${rolesScopeConsentText}'
      },
      'protocolMappers': [
        {
          'id': '241b5ff6-7974-41e5-a77d-d928fe180a27',
          'name': 'audience resolve',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-audience-resolve-mapper',
          'consentRequired': false,
          'config': {}
        },
        {
          'id': 'c4db2509-9319-4b19-91bd-26522f4db7f8',
          'name': 'client roles',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-client-role-mapper',
          'consentRequired': false,
          'config': {
            'user.attribute': 'foo',
            'access.token.claim': 'true',
            'claim.name': 'resource_access.${client_id}.roles',
            'jsonType.label': 'String',
            'multivalued': 'true'
          }
        },
        {
          'id': 'ebe20957-3d39-4c45-8c78-7388ccc5e20d',
          'name': 'realm roles',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-realm-role-mapper',
          'consentRequired': false,
          'config': {
            'user.attribute': 'foo',
            'access.token.claim': 'true',
            'claim.name': 'realm_access.roles',
            'jsonType.label': 'String',
            'multivalued': 'true'
          }
        }
      ]
    },
    {
      'id': 'c42bb53c-9600-4a2e-9efc-a39f4e81fd7d',
      'name': 'web-origins',
      'description': 'OpenID Connect scope for add allowed web origins to the access token',
      'protocol': 'openid-connect',
      'attributes': {
        'include.in.token.scope': 'false',
        'display.on.consent.screen': 'false',
        'consent.screen.text': ''
      },
      'protocolMappers': [
        {
          'id': 'c6e9900a-fab0-44dc-93ad-90d0e15c0863',
          'name': 'allowed web origins',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-allowed-origins-mapper',
          'consentRequired': false,
          'config': {}
        }
      ]
    },
    {
      'id': '15720d05-eb33-40ab-962b-dd31555b52ff',
      'name': 'profile',
      'description': 'OpenID Connect built-in scope: profile',
      'protocol': 'openid-connect',
      'attributes': {
        'include.in.token.scope': 'true',
        'display.on.consent.screen': 'true',
        'consent.screen.text': '${profileScopeConsentText}'
      },
      'protocolMappers': [
        {
          'id': '06a931f2-56dd-42cf-8e26-1c81f00ea057',
          'name': 'website',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-attribute-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'website',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'website',
            'jsonType.label': 'String'
          }
        },
        {
          'id': 'a47cfe1d-f79d-4e65-8013-6b7d830b3fd1',
          'name': 'profile',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-attribute-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'profile',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'profile',
            'jsonType.label': 'String'
          }
        },
        {
          'id': '850a993c-d648-4fed-94ae-0828135e476d',
          'name': 'birthdate',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-attribute-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'birthdate',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'birthdate',
            'jsonType.label': 'String'
          }
        },
        {
          'id': '5e4a5684-52a2-4fb7-968b-abed66846eb1',
          'name': 'locale',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-attribute-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'locale',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'locale',
            'jsonType.label': 'String'
          }
        },
        {
          'id': '794d0e1b-30f3-4f4b-9f49-bd2a3e64c720',
          'name': 'gender',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-attribute-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'gender',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'gender',
            'jsonType.label': 'String'
          }
        },
        {
          'id': 'e8730017-4571-4655-bd21-480ebdc8cc2f',
          'name': 'full name',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-full-name-mapper',
          'consentRequired': false,
          'config': {
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'userinfo.token.claim': 'true'
          }
        },
        {
          'id': '390c0682-ec72-4289-9c78-a7c260f74b8d',
          'name': 'picture',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-attribute-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'picture',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'picture',
            'jsonType.label': 'String'
          }
        },
        {
          'id': '7e9becdb-d33d-4dbe-b112-00a6d22b4065',
          'name': 'given name',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-property-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'firstName',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'given_name',
            'jsonType.label': 'String'
          }
        },
        {
          'id': '460a3cf1-ae4a-48fd-bc53-51ba9e40765a',
          'name': 'middle name',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-attribute-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'middleName',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'middle_name',
            'jsonType.label': 'String'
          }
        },
        {
          'id': '3494de6d-b832-48d9-924b-0c0363334e89',
          'name': 'nickname',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-attribute-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'nickname',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'nickname',
            'jsonType.label': 'String'
          }
        },
        {
          'id': '63995dcd-2928-4f61-8e14-df07692c8330',
          'name': 'username',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-property-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'username',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'preferred_username',
            'jsonType.label': 'String'
          }
        },
        {
          'id': '905baf4f-f1df-4ef8-81a9-42f82829dfc5',
          'name': 'zoneinfo',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-attribute-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'zoneinfo',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'zoneinfo',
            'jsonType.label': 'String'
          }
        },
        {
          'id': 'a8adf48b-023d-4c63-9caf-e7b0ec261bb3',
          'name': 'family name',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-property-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'lastName',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'family_name',
            'jsonType.label': 'String'
          }
        },
        {
          'id': 'e2e497d6-daa0-4306-bf37-59f2de3fcfd2',
          'name': 'updated at',
          'protocol': 'openid-connect',
          'protocolMapper': 'oidc-usermodel-attribute-mapper',
          'consentRequired': false,
          'config': {
            'userinfo.token.claim': 'true',
            'user.attribute': 'updatedAt',
            'id.token.claim': 'true',
            'access.token.claim': 'true',
            'claim.name': 'updated_at',
            'jsonType.label': 'String'
          }
        }
      ]
    }
  ],
  'defaultDefaultClientScopes': [
    'email',
    'role_list',
    'profile',
    'roles',
    'web-origins'
  ],
  'defaultOptionalClientScopes': [
    'address',
    'offline_access',
    'phone',
    'microprofile-jwt'
  ],
  'browserSecurityHeaders': {
    'contentSecurityPolicyReportOnly': '',
    'xContentTypeOptions': 'nosniff',
    'xRobotsTag': 'none',
    'xFrameOptions': 'SAMEORIGIN',
    'contentSecurityPolicy': 'frame-src 'self'; frame-ancestors 'self'; object-src 'none';',
    'xXSSProtection': '1; mode=block',
    'strictTransportSecurity': 'max-age=31536000; includeSubDomains'
  },
  'smtpServer': {},
  'eventsEnabled': false,
  'eventsListeners': [
    'jboss-logging'
  ],
  'enabledEventTypes': [],
  'adminEventsEnabled': false,
  'adminEventsDetailsEnabled': false,
  'identityProviders': [
    {
      'alias': 'github',
      'internalId': 'c16e9969-d11c-4e56-b622-9d81180c8d69',
      'providerId': 'github',
      'enabled': true,
      'updateProfileFirstLoginMode': 'on',
      'trustEmail': false,
      'storeToken': false,
      'addReadTokenRoleOnCreate': false,
      'authenticateByDefault': false,
      'linkOnly': false,
      'firstBrokerLoginFlowAlias': 'first broker login',
      'config': {
        'syncMode': 'IMPORT',
        'clientSecret': '**********',
        'clientId': 'c0933eba815dab764cd6',
        'useJwksUrl': 'true'
      }
    }
  ],
  'identityProviderMappers': [],
  'components': {
    'org.keycloak.services.clientregistration.policy.ClientRegistrationPolicy': [
      {
        'id': '6f9a2895-58f3-483c-9e11-f9c4d8d8b8c0',
        'name': 'Allowed Client Scopes',
        'providerId': 'allowed-client-templates',
        'subType': 'authenticated',
        'subComponents': {},
        'config': {
          'allow-default-scopes': [
            'true'
          ]
        }
      },
      {
        'id': '418f4909-0b2c-4290-8b5d-0645b6d217df',
        'name': 'Max Clients Limit',
        'providerId': 'max-clients',
        'subType': 'anonymous',
        'subComponents': {},
        'config': {
          'max-clients': [
            '200'
          ]
        }
      },
      {
        'id': '8b2e7321-142b-479e-88be-3567c9d2e4cf',
        'name': 'Allowed Protocol Mapper Types',
        'providerId': 'allowed-protocol-mappers',
        'subType': 'authenticated',
        'subComponents': {},
        'config': {
          'allowed-protocol-mapper-types': [
            'oidc-sha256-pairwise-sub-mapper',
            'oidc-address-mapper',
            'oidc-full-name-mapper',
            'saml-role-list-mapper',
            'saml-user-attribute-mapper',
            'saml-user-property-mapper',
            'oidc-usermodel-property-mapper',
            'oidc-usermodel-attribute-mapper'
          ]
        }
      },
      {
        'id': 'a33c1dff-1921-4280-a466-73df9ddb58e0',
        'name': 'Trusted Hosts',
        'providerId': 'trusted-hosts',
        'subType': 'anonymous',
        'subComponents': {},
        'config': {
          'host-sending-registration-request-must-match': [
            'true'
          ],
          'client-uris-must-match': [
            'true'
          ]
        }
      },
      {
        'id': 'b8c3cfc6-3e18-45d0-8c44-407db8b735d3',
        'name': 'Allowed Protocol Mapper Types',
        'providerId': 'allowed-protocol-mappers',
        'subType': 'anonymous',
        'subComponents': {},
        'config': {
          'allowed-protocol-mapper-types': [
            'oidc-full-name-mapper',
            'saml-user-property-mapper',
            'oidc-usermodel-attribute-mapper',
            'saml-user-attribute-mapper',
            'oidc-sha256-pairwise-sub-mapper',
            'oidc-address-mapper',
            'oidc-usermodel-property-mapper',
            'saml-role-list-mapper'
          ]
        }
      },
      {
        'id': 'a2a716b1-b79e-4fc9-bf2a-1aa0649d8da5',
        'name': 'Consent Required',
        'providerId': 'consent-required',
        'subType': 'anonymous',
        'subComponents': {},
        'config': {}
      },
      {
        'id': 'e4eba346-aaab-4638-b155-0d6739aeb8de',
        'name': 'Allowed Client Scopes',
        'providerId': 'allowed-client-templates',
        'subType': 'anonymous',
        'subComponents': {},
        'config': {
          'allow-default-scopes': [
            'true'
          ]
        }
      },
      {
        'id': 'e2225668-37da-43ac-a7a5-d514fa864312',
        'name': 'Full Scope Disabled',
        'providerId': 'scope',
        'subType': 'anonymous',
        'subComponents': {},
        'config': {}
      }
    ],
    'org.keycloak.userprofile.UserProfileProvider': [
      {
        'id': 'b522ec47-8846-445f-88b9-565d8952a803',
        'providerId': 'declarative-user-profile',
        'subComponents': {},
        'config': {}
      }
    ],
    'org.keycloak.keys.KeyProvider': [
      {
        'id': '78df6af7-516f-4764-bfce-8c5f01396146',
        'name': 'hmac-generated',
        'providerId': 'hmac-generated',
        'subComponents': {},
        'config': {
          'priority': [
            '100'
          ],
          'algorithm': [
            'HS256'
          ]
        }
      },
      {
        'id': '7aa933b3-b3e4-4feb-9306-ee40048027ad',
        'name': 'rsa-generated',
        'providerId': 'rsa-generated',
        'subComponents': {},
        'config': {
          'priority': [
            '100'
          ]
        }
      },
      {
        'id': '79ab99d0-d0c0-4748-bb13-f1706f3f0801',
        'name': 'aes-generated',
        'providerId': 'aes-generated',
        'subComponents': {},
        'config': {
          'priority': [
            '100'
          ]
        }
      },
      {
        'id': '30332d03-0a1b-468e-874e-5c764f35efc2',
        'name': 'rsa-enc-generated',
        'providerId': 'rsa-enc-generated',
        'subComponents': {},
        'config': {
          'priority': [
            '100'
          ],
          'algorithm': [
            'RSA-OAEP'
          ]
        }
      }
    ]
  },
  'internationalizationEnabled': false,
  'supportedLocales': [],
  'authenticationFlows': [
    {
      'id': '790c6c4c-9fc2-41a5-9504-656974a53bf7',
      'alias': 'Account verification options',
      'description': 'Method with which to verity the existing account',
      'providerId': 'basic-flow',
      'topLevel': false,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticator': 'idp-email-verification',
          'authenticatorFlow': false,
          'requirement': 'ALTERNATIVE',
          'priority': 10,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticatorFlow': true,
          'requirement': 'ALTERNATIVE',
          'priority': 20,
          'flowAlias': 'Verify Existing Account by Re-authentication',
          'userSetupAllowed': false,
          'autheticatorFlow': true
        }
      ]
    },
    {
      'id': '4962a695-becd-4187-acef-a65b8ffc514d',
      'alias': 'Authentication Options',
      'description': 'Authentication options.',
      'providerId': 'basic-flow',
      'topLevel': false,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticator': 'basic-auth',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 10,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticator': 'basic-auth-otp',
          'authenticatorFlow': false,
          'requirement': 'DISABLED',
          'priority': 20,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticator': 'auth-spnego',
          'authenticatorFlow': false,
          'requirement': 'DISABLED',
          'priority': 30,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        }
      ]
    },
    {
      'id': 'af7da451-a5ac-4d12-8ac5-86e02b105181',
      'alias': 'Browser - Conditional OTP',
      'description': 'Flow to determine if the OTP is required for the authentication',
      'providerId': 'basic-flow',
      'topLevel': false,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticator': 'conditional-user-configured',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 10,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticator': 'auth-otp-form',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 20,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        }
      ]
    },
    {
      'id': 'a5c03930-0864-4b24-b1bd-00cdb6f6eec0',
      'alias': 'Direct Grant - Conditional OTP',
      'description': 'Flow to determine if the OTP is required for the authentication',
      'providerId': 'basic-flow',
      'topLevel': false,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticator': 'conditional-user-configured',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 10,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticator': 'direct-grant-validate-otp',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 20,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        }
      ]
    },
    {
      'id': '030a1219-733c-45f2-981a-54250255af93',
      'alias': 'First broker login - Conditional OTP',
      'description': 'Flow to determine if the OTP is required for the authentication',
      'providerId': 'basic-flow',
      'topLevel': false,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticator': 'conditional-user-configured',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 10,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticator': 'auth-otp-form',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 20,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        }
      ]
    },
    {
      'id': '48072366-1e5a-4e84-bd37-5181a1f7bae2',
      'alias': 'Handle Existing Account',
      'description': 'Handle what to do if there is existing account with same email/username like authenticated identity provider',
      'providerId': 'basic-flow',
      'topLevel': false,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticator': 'idp-confirm-link',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 10,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticatorFlow': true,
          'requirement': 'REQUIRED',
          'priority': 20,
          'flowAlias': 'Account verification options',
          'userSetupAllowed': false,
          'autheticatorFlow': true
        }
      ]
    },
    {
      'id': '493a594d-7726-4182-8117-22e99f85d920',
      'alias': 'Reset - Conditional OTP',
      'description': 'Flow to determine if the OTP should be reset or not. Set to REQUIRED to force.',
      'providerId': 'basic-flow',
      'topLevel': false,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticator': 'conditional-user-configured',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 10,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticator': 'reset-otp',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 20,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        }
      ]
    },
    {
      'id': '31d58513-37ed-4e0c-9db3-eaea3aaa901b',
      'alias': 'User creation or linking',
      'description': 'Flow for the existing/non-existing user alternatives',
      'providerId': 'basic-flow',
      'topLevel': false,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticatorConfig': 'create unique user config',
          'authenticator': 'idp-create-user-if-unique',
          'authenticatorFlow': false,
          'requirement': 'ALTERNATIVE',
          'priority': 10,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticatorFlow': true,
          'requirement': 'ALTERNATIVE',
          'priority': 20,
          'flowAlias': 'Handle Existing Account',
          'userSetupAllowed': false,
          'autheticatorFlow': true
        }
      ]
    },
    {
      'id': 'd295d5c2-259e-4aed-a7b0-15984e6da8ed',
      'alias': 'Verify Existing Account by Re-authentication',
      'description': 'Reauthentication of existing account',
      'providerId': 'basic-flow',
      'topLevel': false,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticator': 'idp-username-password-form',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 10,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticatorFlow': true,
          'requirement': 'CONDITIONAL',
          'priority': 20,
          'flowAlias': 'First broker login - Conditional OTP',
          'userSetupAllowed': false,
          'autheticatorFlow': true
        }
      ]
    },
    {
      'id': '9d2fcf02-84b7-4b45-9321-5ea22b4b5a5a',
      'alias': 'browser',
      'description': 'browser based authentication',
      'providerId': 'basic-flow',
      'topLevel': true,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticator': 'auth-cookie',
          'authenticatorFlow': false,
          'requirement': 'ALTERNATIVE',
          'priority': 10,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticator': 'auth-spnego',
          'authenticatorFlow': false,
          'requirement': 'DISABLED',
          'priority': 20,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticator': 'identity-provider-redirector',
          'authenticatorFlow': false,
          'requirement': 'ALTERNATIVE',
          'priority': 25,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticatorFlow': true,
          'requirement': 'ALTERNATIVE',
          'priority': 30,
          'flowAlias': 'forms',
          'userSetupAllowed': false,
          'autheticatorFlow': true
        }
      ]
    },
    {
      'id': '47318817-bf40-40bb-b7df-b5a6f78b104c',
      'alias': 'clients',
      'description': 'Base authentication for clients',
      'providerId': 'client-flow',
      'topLevel': true,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticator': 'client-secret',
          'authenticatorFlow': false,
          'requirement': 'ALTERNATIVE',
          'priority': 10,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticator': 'client-jwt',
          'authenticatorFlow': false,
          'requirement': 'ALTERNATIVE',
          'priority': 20,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticator': 'client-secret-jwt',
          'authenticatorFlow': false,
          'requirement': 'ALTERNATIVE',
          'priority': 30,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticator': 'client-x509',
          'authenticatorFlow': false,
          'requirement': 'ALTERNATIVE',
          'priority': 40,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        }
      ]
    },
    {
      'id': '1667866a-6659-48a5-8ed8-76d759c9aac5',
      'alias': 'direct grant',
      'description': 'OpenID Connect Resource Owner Grant',
      'providerId': 'basic-flow',
      'topLevel': true,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticator': 'direct-grant-validate-username',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 10,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticator': 'direct-grant-validate-password',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 20,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticatorFlow': true,
          'requirement': 'CONDITIONAL',
          'priority': 30,
          'flowAlias': 'Direct Grant - Conditional OTP',
          'userSetupAllowed': false,
          'autheticatorFlow': true
        }
      ]
    },
    {
      'id': 'e3eff0d3-cca8-418f-a0f4-737f239a5807',
      'alias': 'docker auth',
      'description': 'Used by Docker clients to authenticate against the IDP',
      'providerId': 'basic-flow',
      'topLevel': true,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticator': 'docker-http-basic-authenticator',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 10,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        }
      ]
    },
    {
      'id': 'ef34d055-9824-482a-830d-bfb0e8c9e795',
      'alias': 'first broker login',
      'description': 'Actions taken after first broker login with identity provider account, which is not yet linked to any Keycloak account',
      'providerId': 'basic-flow',
      'topLevel': true,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticatorConfig': 'review profile config',
          'authenticator': 'idp-review-profile',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 10,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticatorFlow': true,
          'requirement': 'REQUIRED',
          'priority': 20,
          'flowAlias': 'User creation or linking',
          'userSetupAllowed': false,
          'autheticatorFlow': true
        }
      ]
    },
    {
      'id': '28178f5c-28bc-4dce-bc35-79168b78f3d7',
      'alias': 'forms',
      'description': 'Username, password, otp and other auth forms.',
      'providerId': 'basic-flow',
      'topLevel': false,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticator': 'auth-username-password-form',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 10,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticatorFlow': true,
          'requirement': 'CONDITIONAL',
          'priority': 20,
          'flowAlias': 'Browser - Conditional OTP',
          'userSetupAllowed': false,
          'autheticatorFlow': true
        }
      ]
    },
    {
      'id': '9563eff6-ee59-4c8f-baf1-d9ee0b0518ba',
      'alias': 'http challenge',
      'description': 'An authentication flow based on challenge-response HTTP Authentication Schemes',
      'providerId': 'basic-flow',
      'topLevel': true,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticator': 'no-cookie-redirect',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 10,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticatorFlow': true,
          'requirement': 'REQUIRED',
          'priority': 20,
          'flowAlias': 'Authentication Options',
          'userSetupAllowed': false,
          'autheticatorFlow': true
        }
      ]
    },
    {
      'id': '6cb134ce-728b-4a2d-8c3d-9258679688df',
      'alias': 'registration',
      'description': 'registration flow',
      'providerId': 'basic-flow',
      'topLevel': true,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticator': 'registration-page-form',
          'authenticatorFlow': true,
          'requirement': 'REQUIRED',
          'priority': 10,
          'flowAlias': 'registration form',
          'userSetupAllowed': false,
          'autheticatorFlow': true
        }
      ]
    },
    {
      'id': '0b0ddd50-ae12-40f9-84ac-b314afa2e19d',
      'alias': 'registration form',
      'description': 'registration form',
      'providerId': 'form-flow',
      'topLevel': false,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticator': 'registration-user-creation',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 20,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticator': 'registration-profile-action',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 40,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticator': 'registration-password-action',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 50,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticator': 'registration-recaptcha-action',
          'authenticatorFlow': false,
          'requirement': 'DISABLED',
          'priority': 60,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        }
      ]
    },
    {
      'id': 'ad947843-5416-4bd3-bc3e-8f7df4da2976',
      'alias': 'reset credentials',
      'description': 'Reset credentials for a user if they forgot their password or something',
      'providerId': 'basic-flow',
      'topLevel': true,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticator': 'reset-credentials-choose-user',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 10,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticator': 'reset-credential-email',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 20,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticator': 'reset-password',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 30,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        },
        {
          'authenticatorFlow': true,
          'requirement': 'CONDITIONAL',
          'priority': 40,
          'flowAlias': 'Reset - Conditional OTP',
          'userSetupAllowed': false,
          'autheticatorFlow': true
        }
      ]
    },
    {
      'id': '41c93501-79c4-4a48-9eab-122ff70b08e0',
      'alias': 'saml ecp',
      'description': 'SAML ECP Profile Authentication Flow',
      'providerId': 'basic-flow',
      'topLevel': true,
      'builtIn': true,
      'authenticationExecutions': [
        {
          'authenticator': 'http-basic-authenticator',
          'authenticatorFlow': false,
          'requirement': 'REQUIRED',
          'priority': 10,
          'userSetupAllowed': false,
          'autheticatorFlow': false
        }
      ]
    }
  ],
  'authenticatorConfig': [
    {
      'id': '2c4fd460-f9de-4813-97d2-c02733312b84',
      'alias': 'create unique user config',
      'config': {
        'require.password.update.after.registration': 'false'
      }
    },
    {
      'id': '3a73c2e1-ecea-4aa5-abc5-bb9b7b7f3f92',
      'alias': 'review profile config',
      'config': {
        'update.profile.on.first.login': 'missing'
      }
    }
  ],
  'requiredActions': [
    {
      'alias': 'CONFIGURE_TOTP',
      'name': 'Configure OTP',
      'providerId': 'CONFIGURE_TOTP',
      'enabled': true,
      'defaultAction': true,
      'priority': 10,
      'config': {}
    },
    {
      'alias': 'terms_and_conditions',
      'name': 'Terms and Conditions',
      'providerId': 'terms_and_conditions',
      'enabled': false,
      'defaultAction': false,
      'priority': 20,
      'config': {}
    },
    {
      'alias': 'UPDATE_PASSWORD',
      'name': 'Update Password',
      'providerId': 'UPDATE_PASSWORD',
      'enabled': true,
      'defaultAction': false,
      'priority': 30,
      'config': {}
    },
    {
      'alias': 'UPDATE_PROFILE',
      'name': 'Update Profile',
      'providerId': 'UPDATE_PROFILE',
      'enabled': true,
      'defaultAction': false,
      'priority': 40,
      'config': {}
    },
    {
      'alias': 'VERIFY_EMAIL',
      'name': 'Verify Email',
      'providerId': 'VERIFY_EMAIL',
      'enabled': true,
      'defaultAction': false,
      'priority': 50,
      'config': {}
    },
    {
      'alias': 'delete_account',
      'name': 'Delete Account',
      'providerId': 'delete_account',
      'enabled': false,
      'defaultAction': false,
      'priority': 60,
      'config': {}
    },
    {
      'alias': 'update_user_locale',
      'name': 'Update User Locale',
      'providerId': 'update_user_locale',
      'enabled': true,
      'defaultAction': false,
      'priority': 1000,
      'config': {}
    }
  ],
  'browserFlow': 'browser',
  'registrationFlow': 'registration',
  'directGrantFlow': 'direct grant',
  'resetCredentialsFlow': 'reset credentials',
  'clientAuthenticationFlow': 'clients',
  'dockerAuthenticationFlow': 'docker auth',
  'attributes': {
    'cibaBackchannelTokenDeliveryMode': 'poll',
    'cibaExpiresIn': '120',
    'cibaAuthRequestedUserHint': 'login_hint',
    'oauth2DeviceCodeLifespan': '600',
    'clientOfflineSessionMaxLifespan': '0',
    'oauth2DevicePollingInterval': '5',
    'clientSessionIdleTimeout': '0',
    'parRequestUriLifespan': '60',
    'clientSessionMaxLifespan': '0',
    'clientOfflineSessionIdleTimeout': '0',
    'cibaInterval': '5'
  },
  'keycloakVersion': '16.1.1',
  'userManagedAccessAllowed': false,
  'clientProfiles': {
    'profiles': []
  },
  'clientPolicies': {
    'policies': []
  }
}";
        }
    }
}
