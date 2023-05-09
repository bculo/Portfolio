using Crypto.Application.Modules.Crypto.Commands.UpdateInfo;
using Crypto.IntegrationTests.Common;
using Crypto.IntegrationTests.Constants;
using Crypto.IntegrationTests.Extensions;
using Crypto.Mock.Common.Clients;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Tests.Common.Extensions;
using Tests.Common.Utilities;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Crypto.IntegrationTests.CryptoController
{
    public class UpdateInfoTests : BaseTests
    {
        private readonly WireMockServer _wireMockServer;

        public UpdateInfoTests(CryptoApiFactory factory) 
            : base(factory)
        {
            _wireMockServer = factory.Services.GetRequiredService<WireMockServer>();
        }

        [Theory]
        [InlineData("BTC")]
        [InlineData("ETH")]
        public async Task PostAsync_ShouldReturnStatusNoContent_WhenSymbolExists(string symbol)
        {
            //Arrange
            var content = HttpClientUtilities.PrepareJsonRequest(new UpdateInfoCommand { Symbol = symbol });
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            var infoResponse = CryptoInfoModelMock.Mock(symbol);

            _wireMockServer
                .Given(WireMock.RequestBuilders.Request.Create().WithPath($"/info"))
                .RespondWith(
                    Response.Create().WithBodyAsJson(infoResponse)
                        .WithStatusCode(HttpStatusCode.OK)
                    );

            //Act
            var response = await _client.PostAsync(ApiEndpoint.CRYPTO_UPDATE_INFO, content);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData("BTC")]
        [InlineData("ETH")]
        public async Task PostAsync_ShouldReturnStatusBadRequest_WhenCryptoInfoClientReturns500(string symbol)
        {
            //Arrange
            var content = HttpClientUtilities.PrepareJsonRequest(new UpdateInfoCommand { Symbol = symbol });
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            var infoResponse = CryptoInfoModelMock.Mock(symbol);

            _wireMockServer
                .Given(WireMock.RequestBuilders.Request.Create().WithPath($"/info"))
                .RespondWith(
                    Response.Create().WithBodyAsJson(infoResponse)
                        .WithStatusCode(HttpStatusCode.InternalServerError)
                    );

            //Act
            var response = await _client.PostAsync(ApiEndpoint.CRYPTO_UPDATE_INFO, content);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("BTC")]
        [InlineData("ETH")]
        public async Task PostAsync_ShouldReturnStatusBadRequest_WhenCryptoInfoClientReturns400(string symbol)
        {
            //Arrange
            var content = HttpClientUtilities.PrepareJsonRequest(new UpdateInfoCommand { Symbol = symbol });
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            var infoResponse = CryptoInfoModelMock.Mock(symbol);

            _wireMockServer
                .Given(WireMock.RequestBuilders.Request.Create().WithPath($"/info"))
                .RespondWith(
                    Response.Create().WithBodyAsJson(infoResponse)
                        .WithStatusCode(HttpStatusCode.BadRequest)
                    );

            //Act
            var response = await _client.PostAsync(ApiEndpoint.CRYPTO_UPDATE_INFO, content);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("----")]
        [InlineData("")]
        public async Task PostAsync_ShouldReturnStatusBadRequest_WhenInvalidSymbolProvided(string symbol)
        {
            //Arrange
            var content = HttpClientUtilities.PrepareJsonRequest(new UpdateInfoCommand { Symbol = symbol });
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            //Act
            var response = await _client.PostAsync(ApiEndpoint.CRYPTO_UPDATE_INFO, content);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("FIL")]
        [InlineData("TDROP")]
        public async Task PostAsync_ShouldReturnStatusBadRequest_WhenSymbolNotFound(string symbol)
        {
            //Arrange
            var content = HttpClientUtilities.PrepareJsonRequest(new UpdateInfoCommand { Symbol = symbol });
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            //Act
            var response = await _client.PostAsync(ApiEndpoint.CRYPTO_UPDATE_INFO, content);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
