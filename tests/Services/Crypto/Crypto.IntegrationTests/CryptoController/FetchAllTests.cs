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
        public async Task Test()
        {
            //Arrange
            HttpClient client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync(ApiEndpoint.FETCH_ALL);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
