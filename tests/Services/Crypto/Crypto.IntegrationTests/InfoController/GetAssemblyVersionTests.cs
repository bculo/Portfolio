using Crypto.IntegrationTests.Constants;
using Crypto.IntegrationTests.Extensions;
using FluentAssertions;
using Tests.Common.Extensions;

namespace Crypto.IntegrationTests.InfoController
{
    [Collection("CryptoCollection")]
    public class GetAssemblyVersionTests : IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly CryptoApiFactory _factory;

        public GetAssemblyVersionTests(CryptoApiFactory factory)
        {
            _factory = factory;
            _client = factory.Client;
        }

        [Theory]
        [InlineData(JwtTokens.USER_ROLE_TOKEN)]
        public async Task GetAsync_ShouldReturnAssemblyVersion_WhenEndpointCalled(string jwtToken)
        {
            //Arrange
            _client.AddJwtToken(jwtToken);

            //Act
            var response = await _client.GetAsync(ApiEndpoint.INFO_VERSION);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var version = await response.Content.ReadAsStringAsync();
            version.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetAsync_ShouldReturnHttpStatus401_WhenJwtTokenNotProvided()
        {
            //Arrange
            _client.RemoveJwtToken();

            //Act
            var response = await _client.GetAsync(ApiEndpoint.INFO_VERSION);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            await _factory.ResetDatabaseAsync();
        }
    }
}
