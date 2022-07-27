using FluentAssertions;
using Keycloak.Common.Clients;
using Keycloak.Common.Options;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.UnitTests
{
    public class KeycloakAdminClientTests
    {
        private const string VALID_REALM = "PortfolioRealm";
        private const string VALID_ACCESS_TOKEN = "abc-sir";
        private const string VALID_USER_ID = "e246ce6c-30de-4b82-ba92-cafb31bdc9a0";

        [Fact]
        public async Task GetUsers_ShouldReturnUserList_WhenValidRealmAndAccessTokenProvided()
        {
            //Arrange
            string realm = VALID_REALM;
            string token = VALID_ACCESS_TOKEN;
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
            string token = VALID_ACCESS_TOKEN;
            var adminClient = CreateAdminClientUsers(realm, token);

            //Act
            await Assert.ThrowsAsync<ArgumentNullException>(() => adminClient.GetUsers(realm, token));
        }

        [Fact]
        public async Task GetUsers_ShouldReturnNull_WhenInvalidRealmProvided()
        {
            //Arrange
            string realm = "invalid-realm";
            string token = VALID_ACCESS_TOKEN;
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
            string token = VALID_ACCESS_TOKEN;
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
            string token = VALID_ACCESS_TOKEN;
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
            string token = VALID_ACCESS_TOKEN;
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
            string token = VALID_ACCESS_TOKEN;
            string userId = Guid.NewGuid().ToString();
            var adminClient = CreateAdminClientForSingleUser(realm, token, userId);

            //Act
            var response = await adminClient.GetUserById(realm, token, Guid.Parse(userId));

            //Assert
            response.Should().BeNull();
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
            string adminApiBaseUrl = "http://localhost:8080/auth/admin/realms/";

            var mockHandler = new MockHttpMessageHandler();

            if(realm == VALID_REALM && token == VALID_ACCESS_TOKEN)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond("application/json", GetValidUsersResponse());
            }

            if(realm != VALID_REALM && token == VALID_ACCESS_TOKEN)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.NotFound, "application/json", "'error': 'Realm not found.'");
            }

            if (realm == VALID_REALM && token != VALID_ACCESS_TOKEN)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.Unauthorized, "application/json", "'error': 'HTTP 401 Unauthorized'");
            }

            if(realm != VALID_REALM && token != VALID_ACCESS_TOKEN)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond(HttpStatusCode.BadRequest);
            }

            var factoryMock = new Mock<IHttpClientFactory>();
            factoryMock.Setup(i => i.CreateClient(It.IsAny<string>()))
                .Returns(mockHandler.ToHttpClient());

            var options = Microsoft.Extensions.Options.Options.Create(new KeycloakAdminApiOptions
            {
                AdminApiEndpointBase = adminApiBaseUrl,
            });

            var logger = Mock.Of<ILogger<KeycloakAdminClient>>();

            return new KeycloakAdminClient(factoryMock.Object, options, logger);
        }

        private string GetValidUsersResponse()
        {
            return @"[
                {
                    'id': 'e246ce6c-30de-4b82-ba92-cafb31bdc9a0',
                    'createdTimestamp': 1657999153525,
                    'username': 'bculo',
                    'enabled': true,
                    'totp': true,
                    'emailVerified': false,
                    'firstName': 'Božo',
                    'lastName': 'Čulo',
                    'email': 'culobozo@gmail.com',
                    'disableableCredentialTypes': [],
                    'requiredActions': [],
                    'notBefore': 0,
                    'access': {
                        'manageGroupMembership': true,
                        'view': true,
                        'mapRoles': true,
                        'impersonate': true,
                        'manage': true
                    }
                },
                {
                    'id': 'be2b67bb-0a15-42d4-8c28-b4729a4329b6',
                    'createdTimestamp': 1657819590270,
                    'username': 'culix',
                    'enabled': true,
                    'totp': false,
                    'emailVerified': false,
                    'disableableCredentialTypes': [],
                    'requiredActions': [],
                    'notBefore': 0,
                    'access': {
                        'manageGroupMembership': true,
                        'view': true,
                        'mapRoles': true,
                        'impersonate': true,
                        'manage': true
                    }
                },
                {
                    'id': 'f534a4a8-ff20-4a0e-85bf-d79fac599c78',
                    'createdTimestamp': 1657993399744,
                    'username': 'dorix',
                    'enabled': true,
                    'totp': false,
                    'emailVerified': false,
                    'firstName': 'dorix',
                    'lastName': 'morix',
                    'email': 'dorix@gmail.com',
                    'disableableCredentialTypes': [],
                    'requiredActions': [],
                    'notBefore': 0,
                    'access': {
                        'manageGroupMembership': true,
                        'view': true,
                        'mapRoles': true,
                        'impersonate': true,
                        'manage': true
                    }
                },
                {
                    'id': 'acdd8e0e-2e20-4304-90de-6426b05a6af6',
                    'createdTimestamp': 1657991769743,
                    'username': 'hardcorenoob',
                    'enabled': true,
                    'totp': true,
                    'emailVerified': true,
                    'firstName': 'mulix',
                    'lastName': 'mulix',
                    'email': 'hardcorenoob2@gmail.com',
                    'disableableCredentialTypes': [],
                    'requiredActions': [],
                    'notBefore': 0,
                    'access': {
                        'manageGroupMembership': true,
                        'view': true,
                        'mapRoles': true,
                        'impersonate': true,
                        'manage': true
                    }
                },
                {
                    'id': '47a9594d-1dbf-41c4-a77c-0be19d50537d',
                    'createdTimestamp': 1657997777837,
                    'username': 'marinko',
                    'enabled': true,
                    'totp': false,
                    'emailVerified': false,
                    'firstName': 'Marinko',
                    'lastName': 'Stanić',
                    'email': 'marinkosvaser@gmail.com',
                    'disableableCredentialTypes': [],
                    'requiredActions': [
                        'CONFIGURE_TOTP'
                    ],
                    'notBefore': 0,
                    'access': {
                        'manageGroupMembership': true,
                        'view': true,
                        'mapRoles': true,
                        'impersonate': true,
                        'manage': true
                    }
                }
            ]";
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
            string adminApiBaseUrl = "http://localhost:8080/auth/admin/realms/";

            var handerMocked = false;

            var mockHandler = new MockHttpMessageHandler();

            //VALID DATA
            if (realm == VALID_REALM && token == VALID_ACCESS_TOKEN && userId == VALID_USER_ID)
            {
                mockHandler.When(HttpMethod.Get, "*").Respond("application/json", GetValidSingleUserResponse());
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

            var factoryMock = new Mock<IHttpClientFactory>();
            factoryMock.Setup(i => i.CreateClient(It.IsAny<string>()))
                .Returns(mockHandler.ToHttpClient());

            var options = Microsoft.Extensions.Options.Options.Create(new KeycloakAdminApiOptions
            {
                AdminApiEndpointBase = adminApiBaseUrl,
            });

            var logger = Mock.Of<ILogger<KeycloakAdminClient>>();

            return new KeycloakAdminClient(factoryMock.Object, options, logger);
        }

        private string GetValidSingleUserResponse()
        {
            return @"{
                'id': 'e246ce6c-30de-4b82-ba92-cafb31bdc9a0',
                'createdTimestamp': 1657999153525,
                'username': 'bculo',
                'enabled': true,
                'totp': true,
                'emailVerified': false,
                'firstName': 'Božo',
                'lastName': 'Čulo',
                'email': 'culobozo@gmail.com',
                'disableableCredentialTypes': [],
                'requiredActions': [],
                'federatedIdentities': [
                    {
                        'identityProvider': 'github',
                        'userId': '37018801',
                        'userName': 'bculo'
                    }
                ],
                'notBefore': 0,
                'access': {
                    'manageGroupMembership': true,
                    'view': true,
                    'mapRoles': true,
                    'impersonate': true,
                    'manage': true
                }
            }";
        }
    }
}
