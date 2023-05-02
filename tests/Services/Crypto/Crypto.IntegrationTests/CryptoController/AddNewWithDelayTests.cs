using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Crypto.IntegrationTests.Common;
using Crypto.IntegrationTests.Constants;
using Crypto.IntegrationTests.Extensions;
using FluentAssertions;
using Tests.Common.Extensions;
using Tests.Common.Utilities;

namespace Crypto.IntegrationTests.CryptoController
{
    public class AddNewWithDelayTests : BaseTests
    {
        public AddNewWithDelayTests(CryptoApiFactory factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("BTC")]
        [InlineData("ETH")]
        public async Task PostAsync_ShouldReturnStatusOK_WhenSymbolAlreadyExists(string symbol)
        {
            //Arrange
            var content = HttpClientUtilities.PrepareJsonRequest(new AddNewCommand { Symbol = symbol });
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            //Act
            var response = await _client.PostAsync(ApiEndpoint.CRYPTO_ADD_NEW_DELAY, content);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
