using Crypto.Application.Modules.Crypto.Queries.FetchAll;
using Crypto.Infrastracture.Persistence;
using Crypto.IntegrationTests.Constants;
using FluentAssertions;
using Newtonsoft.Json;

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
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var instances = JsonConvert.DeserializeObject<List<FetchAllResponseDto>>(jsonResponse);
            instances.Should().AllBeOfType<FetchAllResponseDto>();
        }
    }
}
