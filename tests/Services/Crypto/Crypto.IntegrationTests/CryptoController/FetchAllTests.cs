using Crypto.Infrastracture.Persistence;
using Crypto.IntegrationTests.Constants;
using FluentAssertions;

namespace Crypto.IntegrationTests.CryptoController
{
    public class FetchAllTests : IClassFixture<CryptoApiFactory>
    {
        private readonly CryptoApiFactory _factory;

        public FetchAllTests(CryptoApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task FetchAll_ShouldReturnStatusOk_WhenExecutedSuccessfully()
        {
            //Arrange
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync(ApiEndpoint.CRYPTO_FETCH_ALL);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
