using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Crypto.Application.Modules.Crypto.Commands.UndoNewWithDelay;
using Crypto.IntegrationTests.Constants;
using Crypto.IntegrationTests.Extensions;
using Crypto.IntegrationTests.Utils;
using FluentAssertions;

namespace Crypto.IntegrationTests.CryptoController
{
    [Collection("CryptoCollection")]
    public class UndoAddNewDelayTests : IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly CryptoApiFactory _factory;

        public UndoAddNewDelayTests(CryptoApiFactory factory)
        {
            _factory = factory;
            _client = factory.Client;
        }

        [Fact]
        public async Task PostAsync_ShouldReturnStatusOK_WhenSymbolAlreadyExists()
        {
            //Arrange
            var content = HttpClientUtilities.PrepareJsonRequest(new UndoNewWithDelayCommand { TemporaryId = Guid.NewGuid() });
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            //Act
            var response = await _client.PostAsync(ApiEndpoint.CRYPTO_ADD_UNDO_DELAY, content);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            await _factory.ResetDatabaseAsync();
        }
    }
}
