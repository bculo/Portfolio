using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Crypto.IntegrationTests.Constants;
using Crypto.IntegrationTests.Extensions;
using Crypto.IntegrationTests.Utils;
using FluentAssertions;

namespace Crypto.IntegrationTests.CryptoController
{
    [Collection("CryptoCollection")]
    public class AddNewWithDelayTests : IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly CryptoApiFactory _factory;

        public AddNewWithDelayTests(CryptoApiFactory factory)
        {
            _factory = factory;
            _client = factory.Client;
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

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            await _factory.ResetDatabaseAsync();
        }
    }
}
