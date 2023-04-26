using Crypto.Application.Modules.Crypto.Commands.UpdateInfo;
using Crypto.IntegrationTests.Common;
using Crypto.IntegrationTests.Constants;
using Crypto.IntegrationTests.Extensions;
using Crypto.IntegrationTests.Utils;
using FluentAssertions;

namespace Crypto.IntegrationTests.CryptoController
{
    [Collection("CryptoCollection")]
    public class UpdateInfoTests : BaseTests
    {
        public UpdateInfoTests(CryptoApiFactory factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("BTC")]
        [InlineData("ETH")]
        public async Task PostAsync_ShouldReturnStatusNoContent_WhenSymbolExists(string symbol)
        {
            //Arrange
            var content = HttpClientUtilities.PrepareJsonRequest(new UpdateInfoCommand { Symbol = symbol });
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            //Act
            var response = await _client.PostAsync(ApiEndpoint.CRYPTO_UPDATE_INFO, content);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
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
