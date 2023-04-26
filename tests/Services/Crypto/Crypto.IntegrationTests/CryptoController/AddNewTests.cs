using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Crypto.IntegrationTests.Common;
using Crypto.IntegrationTests.Constants;
using Crypto.IntegrationTests.Extensions;
using Crypto.IntegrationTests.Utils;
using FluentAssertions;

namespace Crypto.IntegrationTests.CryptoController
{
    [Collection("CryptoCollection")]
    public class AddNewTests : BaseTests
    {

        public AddNewTests(CryptoApiFactory factory) 
            : base(factory)
        {
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

            //Act
            var response = await _client.PostAsync(ApiEndpoint.CRYPTO_ADD_NEW, content);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
    }
}
