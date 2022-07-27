using FluentAssertions;
using Keycloak.Common.Clients;
using Keycloak.Common.Options;
using Keycloak.Common.UnitTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    public class KeycloakCredentialFlowClientTests
    {
        public const string VALID_CLIENT_ID = "VALID.CLIENT";
        public const string VALID_CLIENT_SECRET = "v9YmKiVQw86L69fYXhyP2B3WdRiXbKSc";

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
            string authorizationServerUrl = "http://authorizationpoint/";

            var handler = new MockHttpMessageHandler();
            if(clientSecret == VALID_CLIENT_SECRET && clientId == VALID_CLIENT_ID)
            {
                string validResponse = File.ReadAllText("Static/client-credentials-valid.json");
                handler.When(HttpMethod.Post, "*").Respond("application/json", validResponse);
            }
            else
            {
                handler.When(HttpMethod.Post, "*").Respond(HttpStatusCode.BadRequest);
            }

            var factoryMock = new Mock<IHttpClientFactory>();
            factoryMock.Setup(i => i.CreateClient(It.IsAny<string>()))
                .Returns(handler.ToHttpClient());

            var options = Microsoft.Extensions.Options.Options.Create(new KeycloakClientCredentialFlowOptions
            {
                AuthorizationServerUrl = authorizationServerUrl,
            });

            var logger = Mock.Of<ILogger<KeycloakCredentialFlowClient>>();

            return new KeycloakCredentialFlowClient(factoryMock.Object, options, logger);
        }
    }
}
