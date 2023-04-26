using Crypto.Application.Modules.Crypto.Commands.UpdatePrice;
using Crypto.IntegrationTests.Common;
using Crypto.IntegrationTests.Constants;
using Crypto.IntegrationTests.Extensions;
using Crypto.IntegrationTests.Utils;
using FluentAssertions;
using Newtonsoft.Json;
using System.Text;

namespace Crypto.IntegrationTests.CryptoController
{
    [Collection("CryptoCollection")]
    public class UpdatePriceTests : BaseTests
    {
        public UpdatePriceTests(CryptoApiFactory factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("FIL")]
        [InlineData("TDROP")]
        public async Task PostAsync_ShouldReturnStatusBadRequest_WhenNonexistentSymbolProvided(string symbol)
        {
            var content = HttpClientUtilities.PrepareJsonRequest(new UpdatePriceCommand { Symbol = symbol });
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            var response = await _client.PostAsync(ApiEndpoint.CRYPTO_UPDATE_PRICE, content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("----")]
        [InlineData("")]
        public async Task PostAsync_ShouldReturnStatusBadRequest_WhenInvalidSymbolProvided(string symbol)
        {
            //Arrange
            var content = HttpClientUtilities.PrepareJsonRequest(new UpdatePriceCommand { Symbol = symbol });
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            //Act
            var response = await _client.PostAsync(ApiEndpoint.CRYPTO_UPDATE_PRICE, content);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("BTC")]
        [InlineData("ETH")]
        public async Task PostAsync_ShouldReturnStatusNoContent_WhenSymbolExists(string symbol)
        {
            //Arrange
            var content = HttpClientUtilities.PrepareJsonRequest(new UpdatePriceCommand { Symbol = symbol });
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            //Act
            var response = await _client.PostAsync(ApiEndpoint.CRYPTO_UPDATE_INFO, content);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
    }
}
