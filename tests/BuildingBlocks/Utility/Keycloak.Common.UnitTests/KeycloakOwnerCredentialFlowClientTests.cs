using Auth0.Abstract.Models;
using AutoFixture;
using FluentAssertions;
using Keycloak.Common.Clients;
using Keycloak.Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common.UnitTests
{
    public class KeycloakOwnerCredentialFlowClientTests
    {
        public const string AUTHORIZATION_SERVER = "http://localhost:8080/auth/realms/PortfolioRealm/";
        private const string VALID_CLIENTID = "Test.Client"; //Access_Type should be public
        private const string VALID_USERNAME = "dorix";
        private const string VALID_PASSWORD = "dorix";

        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<IHttpClientFactory> _mockFactory = new Mock<IHttpClientFactory>();
        private readonly IOptions<KeycloakOwnerCredentialFlowOptions> _options;
        private readonly ILogger<KeycloakOwnerCredentialFlowClient> _logger = Mock.Of<ILogger<KeycloakOwnerCredentialFlowClient>>();

        public KeycloakOwnerCredentialFlowClientTests()
        {
            _options = Microsoft.Extensions.Options.Options.Create(new KeycloakOwnerCredentialFlowOptions
            {
                AuthorizationServerUrl = AUTHORIZATION_SERVER,
            });
        }

        [Fact]
        public async Task GetToken_ShouldReturnResponse_WhenValidUserProvided()
        {
            //Arrange
            string clientid = VALID_CLIENTID;
            string username = VALID_USERNAME;
            string password = VALID_PASSWORD;
            var client = CreateClient(clientid, username, password);

            //Act
            var response = await client.GetToken(clientid, username, password);

            //Assert
            response.Should().NotBeNull();
            response.AccessToken.Should().NotBeEmpty();
            response.ExpiresIn.Should().BeGreaterThan(0);
            response.RefreshToken.Should().NotBeEmpty();
            response.RefreshTokenExpiresIn.Should().BeGreaterThan(0);
            response.TokenType.Should().NotBeEmpty().And.BeEquivalentTo("Bearer");
        }

        [Fact]
        public async Task GetToken_ShouldReturnNull_WhenInvalidUserProvided()
        {
            //Arrange
            string clientid = VALID_CLIENTID;
            string username = "wrong_user";
            string password = "wrong_user";
            var client = CreateClient(clientid, username, password);

            //Act
            var response = await client.GetToken(clientid, username, password);

            //Assert
            response.Should().BeNull();
        }

        [Fact]
        public async Task GetToken_ShouldReturnNull_WhenInvalidClientIdProvided()
        {
            //Arrange
            string clientid = "wrong_client_id";
            string username = VALID_USERNAME;
            string password = VALID_PASSWORD;
            var client = CreateClient(clientid, username, password);

            //Act
            var response = await client.GetToken(clientid, username, password);

            //Assert
            response.Should().BeNull();
        }

        [Fact]
        public async Task GetToken_ShouldReturnNull_WhenInvalidClientAndUserProvided()
        {
            //Arrange
            string clientid = "wrong_client_id";
            string username = "wrong_user";
            string password = "wrong_user";
            var client = CreateClient(clientid, username, password);

            //Act
            var response = await client.GetToken(clientid, username, password);

            //Assert
            response.Should().BeNull();
        }

        [Fact]
        public async Task GetToken_ShouldThrowArgumentNullException_WhenAnNullParameterPassed()
        {
            //Arrange
            string clientid = null;
            string username = VALID_USERNAME;
            string password = VALID_PASSWORD;
            var client = CreateClient(clientid, username, password);

            //Act and Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => client.GetToken(clientid, username, password));
        }

        private KeycloakOwnerCredentialFlowClient CreateClient(string clientid, string username, string password)
        {
            var handler = new MockHttpMessageHandler();
            if (clientid == VALID_CLIENTID && username == VALID_USERNAME && password == VALID_PASSWORD)
            {
                var responseInstnace = _fixture.Create<TokenAuthorizationCodeResponse>();
                responseInstnace.TokenType = "Bearer";
                handler.When(HttpMethod.Post, "*").Respond("application/json", JsonConvert.SerializeObject(responseInstnace));
            }
            else
            {
                handler.When(HttpMethod.Post, "*").Respond(HttpStatusCode.BadRequest);
            }

            _mockFactory.Setup(i => i.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient());

            return new KeycloakOwnerCredentialFlowClient(_mockFactory.Object, _options, _logger);
        }
    }
}
