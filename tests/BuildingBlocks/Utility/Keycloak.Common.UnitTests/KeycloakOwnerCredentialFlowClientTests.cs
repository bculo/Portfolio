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
    public class KeycloakOwnerCredentialFlowClientTests
    {
        private const string VALID_CLIENTID = "Trend.Client";
        private const string VALID_USERNAME = "dorix";
        private const string VALID_PASSWORD = "dorix";

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
            string authorizationServerUrl = "http://localhost:8080/auth/realms/PortfolioRealm/";

            var handler = new MockHttpMessageHandler();
            if (clientid == VALID_CLIENTID && username == VALID_USERNAME && password == VALID_PASSWORD)
            {
                handler.When(HttpMethod.Post, "*").Respond("application/json", GetValidJsonResponse());
            }
            else
            {
                handler.When(HttpMethod.Post, "*").Respond(HttpStatusCode.BadRequest);
            }

            var factoryMock = new Mock<IHttpClientFactory>();
            factoryMock.Setup(i => i.CreateClient(It.IsAny<string>()))
                .Returns(handler.ToHttpClient());

            var options = Microsoft.Extensions.Options.Options.Create(new KeycloakOwnerCredentialFlowOptions
            {
                AuthorizationServerUrl = authorizationServerUrl,
            });

            var logger = Mock.Of<ILogger<KeycloakOwnerCredentialFlowClient>>();

            return new KeycloakOwnerCredentialFlowClient(factoryMock.Object, options, logger);
        }

        private string GetValidJsonResponse()
        {
            return @"{
                'access_token': 'eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJiN0RWUWJOaDdQVnJZTHF5SEField6dTV6TFc2dXQ5MGFoczI2LUFLU2tZIn0.eyJleHAiOjE2NTg5MDgzNjIsImlhdCI6MTY1ODkwODA2MiwianRpIjoiY2Y1ODJlMGQtZDkwNS00OGUxLWIwODYtYTE3YmUzODAzMmJhIiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL2F1dGgvcmVhbG1zL1BvcnRmb2xpb1JlYWxtIiwiYXVkIjoiYWNjb3VudCIsInN1YiI6ImY1MzRhNGE4LWZmMjAtNGEwZS04NWJmLWQ3OWZhYzU5OWM3OCIsInR5cCI6IkJlYXJlciIsImF6cCI6IlRyZW5kLkNsaWVudCIsInNlc3Npb25fc3RhdGUiOiI5ZWIzODNlNS1mMWEzLTRjZmUtYTBiOC05YWYyZTkzZGYwZjUiLCJhY3IiOiIxIiwiYWxsb3dlZC1vcmlnaW5zIjpbImh0dHA6Ly9sb2NhbGhvc3Q6NDIwMCJdLCJyZWFsbV9hY2Nlc3MiOnsicm9sZXMiOlsiZGVmYXVsdC1yb2xlcy1wb3J0Zm9saW9yZWFsbSIsIlVzZXIiLCJvZmZsaW5lX2FjY2VzcyIsInVtYV9hdXRob3JpemF0aW9uIl19LCJyZXNvdXJjZV9hY2Nlc3MiOnsiVHJlbmQuQ2xpZW50Ijp7InJvbGVzIjpbIlVzZXIiXX0sImFjY291bnQiOnsicm9sZXMiOlsibWFuYWdlLWFjY291bnQiLCJtYW5hZ2UtYWNjb3VudC1saW5rcyIsInZpZXctcHJvZmlsZSJdfX0sInNjb3BlIjoiZW1haWwgcHJvZmlsZSIsInNpZCI6IjllYjM4M2U1LWYxYTMtNGNmZS1hMGI4LTlhZjJlOTNkZjBmNSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwibmFtZSI6ImRvcml4IG1vcml4IiwicHJlZmVycmVkX3VzZXJuYW1lIjoiZG9yaXgiLCJnaXZlbl9uYW1lIjoiZG9yaXgiLCJmYW1pbHlfbmFtZSI6Im1vcml4IiwiZW1haWwiOiJkb3JpeEBnbWFpbC5jb20ifQ.kLRLRgGXqT3CfFNivvjTQ0rfHHgmpTbkFylPnY8qC33IWKpIWS00AnCGeLJZHL_38LzCTnWTcZSyuEV2QWPZF4x8dzGt-A1BF915lYN28mLTvSD0vzzh1KNYh32vs2hKIUksMHL4mzU37CuK4FfoG7Dgg1gAbJA6J9BWMeGdiTxBSv55eYNB-eVFgGLhzwcNv7FTy5UXI2k_QeBKIo4EuSFsOZ35YOTPe3xWkU7ocTHcoot3h54Dmg6baHnj_0LYHCl45f3j3P2CwQJN-TJ72ZNfhQK12N5IWeZIciw7uOolurNmDhhuSv-kdv77tG5bmSzNJXBZCRBdVYnS7WqzbA',
                'expires_in': 300,
                'refresh_expires_in': 1800,
                'refresh_token': 'eyJhbGciOiJIUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJlZWVmNjI3Ni1kNDJiLTQwMWEtOGJiMy1kZWNiNGQxN2ZkYTgifQ.eyJleHAiOjE2NTg5MDk4NjIsImlhdCI6MTY1ODkwODA2MiwianRpIjoiNDIxNWIxYWUtZDNlMy00OGFlLTliNTYtMjRhODNmY2I1MmZhIiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL2F1dGgvcmVhbG1zL1BvcnRmb2xpb1JlYWxtIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo4MDgwL2F1dGgvcmVhbG1zL1BvcnRmb2xpb1JlYWxtIiwic3ViIjoiZjUzNGE0YTgtZmYyMC00YTBlLTg1YmYtZDc5ZmFjNTk5Yzc4IiwidHlwIjoiUmVmcmVzaCIsImF6cCI6IlRyZW5kLkNsaWVudCIsInNlc3Npb25fc3RhdGUiOiI5ZWIzODNlNS1mMWEzLTRjZmUtYTBiOC05YWYyZTkzZGYwZjUiLCJzY29wZSI6ImVtYWlsIHByb2ZpbGUiLCJzaWQiOiI5ZWIzODNlNS1mMWEzLTRjZmUtYTBiOC05YWYyZTkzZGYwZjUifQ.cZQKmtzWx9VuabElf0TpmaWntlEcDKHleHfpJSKcs08',
                'token_type': 'Bearer',
                'not-before-policy': 1657889614,
                'session_state': '9eb383e5-f1a3-4cfe-a0b8-9af2e93df0f5',
                'scope': 'email profile'
            }";
        }
    }
}
