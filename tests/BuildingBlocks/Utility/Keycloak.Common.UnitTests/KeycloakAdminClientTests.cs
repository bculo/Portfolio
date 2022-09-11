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
            var accessToken = await GetAccessToken();
            var client = CreateClient(accessToken, ImportRealmValidScenario, null);

            var json = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Static", "create-realm-example.json"));

            var response = await client.ImportRealm(json, accessToken);

            response.Should().Be(true);
        }

        private (bool, string) ImportRealmValidScenario()
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

            _httpOwnerClientFactory.Setup(i => i.CreateClient(It.IsAny<string>())).Returns(mockHandler.ToHttpClient());
            var authClient = new KeycloakOwnerCredentialFlowClient(_httpOwnerClientFactory.Object, _ownerApiOptions, _loggerOwner);

            var response = await authClient.GetToken("admin-cli", "admin", "admin");
            return response.AccessToken;
        }

        private KeycloakAdminClient CreateClient(string token, Func<(bool valid, string json)> func, string? realm)
        {
            var handerMocked = false;
            var mockHandler = new MockHttpMessageHandler();

            var (valid, json) = func();

            if ((realm == VALID_REALM || realm == null) && token == VALID_ACCESS_TOKEN && valid)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.OK, "application/json", json);
                handerMocked = true;
            }

            //INVALID REALM
            if ((realm != VALID_REALM && realm != null) && token == VALID_ACCESS_TOKEN && valid)
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
            if (!valid)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.BadRequest);
            }

            _httpAdminApiClientFactory.Setup(i => i.CreateClient(It.IsAny<string>()))
                .Returns(mockHandler.ToHttpClient());

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
    }
}
