using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Crypto.IntegrationTests.Common;
using Crypto.IntegrationTests.Constants;
using Crypto.Mock.Common.Clients;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Tests.Common.Extensions;
using Tests.Common.Utilities;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Crypto.IntegrationTests.CryptoController
{
    public class AddNewTests : BaseTests
    {
        private readonly WireMockServer _wireMockServer;
        
        public AddNewTests(CryptoApiFactory factory) 
            : base(factory)
        {
            _wireMockServer = factory.Services.GetRequiredService<WireMockServer>();
        }

        [Fact]
        public async Task PostAsync_ShouldReturnStatusBadRequest_WhenSymbolAlreadyExists()
        {
            //Arrange
            string symbol = "BTC";
            var content = HttpClientUtilities.PrepareJsonRequest(new AddNewCommand { Symbol = symbol });
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            //Act
            var response = await _client.PostAsync(ApiEndpoint.CRYPTO_ADD_NEW, content);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("BTC2")]
        [InlineData("---!-")]
        [InlineData("ETHIIIIIIIIIIIIIIIIIIIIIIIII")]
        public async Task PostAsync_ShouldReturnStatusBadRequest_WhenSymbolHasInvalidFormat(string symbol)
        {
            //Arrange
            var content = HttpClientUtilities.PrepareJsonRequest(new AddNewCommand { Symbol = symbol });
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);
            
            //Act
            var response = await _client.PostAsync(ApiEndpoint.CRYPTO_ADD_NEW, content);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("MATIC")]
        [InlineData("BNB")]
        public async Task PostAsync_ShouldReturnStatusNoContent_WhenNonexistentValidSymbolProvided(string symbol)
        {
            //Arrange
            var content = HttpClientUtilities.PrepareJsonRequest(new AddNewCommand { Symbol = symbol });
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            var infoResponse = CryptoInfoModelMock.Mock(symbol);
            
            _wireMockServer
                .Given(Request.Create().WithPath($"/info"))
                .RespondWith(
                    Response.Create().WithBodyAsJson(infoResponse)
                        .WithStatusCode(HttpStatusCode.OK)
                    );
            
            //Act
            var response = await _client.PostAsync(ApiEndpoint.CRYPTO_ADD_NEW, content);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
