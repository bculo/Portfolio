using Auth0.Abstract.Models;
using AutoFixture;
using Keycloak.Common.Clients;
using Keycloak.Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using System.Net;

namespace Keycloak.Common.UnitTests
{
    public class KeycloakCredentialFlowClientTests
    {
        public const string AUTHORIZATION_SERVER = "http://authorizationpoint/";
        public const string VALID_CLIENT_ID = "VALID.CLIENT";
        public const string VALID_CLIENT_SECRET = "v9YmKiVQw86L69fYXhyP2B3WdRiXbKSc";

        private readonly Fixture _fixture = new Fixture();
        private readonly Mock<IHttpClientFactory> _mockFactory = new Mock<IHttpClientFactory>();
        private readonly IOptions<KeycloakClientCredentialFlowOptions> _options;
        private readonly ILogger<KeycloakCredentialFlowClient> _logger = Mock.Of<ILogger<KeycloakCredentialFlowClient>>();

        public KeycloakCredentialFlowClientTests()
        {
            _options = Microsoft.Extensions.Options.Options.Create(new KeycloakClientCredentialFlowOptions
            {
                AuthorizationServerUrl = AUTHORIZATION_SERVER,
            });
        }

        [Fact]
        public async Task GetToken_ShouldReturnToken_WhenValidClientDataProvided()
        {
            var clientId = VALID_CLIENT_ID;
            var clientSecret = VALID_CLIENT_SECRET;
            var instance = CreateFlowClientInstance(clientId, clientSecret);

            var tokenResponse = await instance.GetToken(clientId, clientSecret);

            Assert.NotNull(tokenResponse);
            Assert.NotEmpty(tokenResponse.TokenType);
        }

        [Fact]
        public async Task GetToken_ShouldThrowException_WhenNullClientDataProvided()
        {
            string? clientId = null;
            string? clientSecret = null;
            var instance = CreateFlowClientInstance(clientId, clientSecret);

            await Assert.ThrowsAsync<ArgumentNullException>(() => instance.GetToken(clientId, clientSecret));
        }

        [Fact]
        public async Task GetToken_ShouldReturnNull_When_InvalidClientDataProvided()
        {
            string? clientId = "random.client";
            string? clientSecret = "randomrandomrandomabc";
            var instance = CreateFlowClientInstance(clientId, clientSecret);

            var tokenResponse = await instance.GetToken(clientId, clientSecret);

            Assert.Null(tokenResponse);
        }

        internal KeycloakCredentialFlowClient CreateFlowClientInstance(string clientId, string clientSecret)
        {
            var handler = new MockHttpMessageHandler();

            if(clientSecret == VALID_CLIENT_SECRET && clientId == VALID_CLIENT_ID)
            {
                string validResponse = JsonConvert.SerializeObject(_fixture.Create<TokenClientCredentialResponse>());
                handler.When(HttpMethod.Post, "*").Respond("application/json", validResponse);
            }
            else
            {
                handler.When(HttpMethod.Post, "*").Respond(HttpStatusCode.BadRequest);
            }

            _mockFactory.Setup(i => i.CreateClient(It.IsAny<string>()))
                .Returns(handler.ToHttpClient());

            return new KeycloakCredentialFlowClient(_mockFactory.Object, _options, _logger);
        }
    }
}
