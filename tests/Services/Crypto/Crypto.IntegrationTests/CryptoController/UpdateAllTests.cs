using Crypto.IntegrationTests.Constants;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.CryptoController
{
    public class UpdateAllTests : IClassFixture<CryptoApiFactory>
    {
        private readonly CryptoApiFactory _factory;

        public UpdateAllTests(CryptoApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task UpdateAllPrices_ShouldReturnStatusNoContent_WhenExistingSymbolProvided()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(ApiEndpoint.CRYPTO_UPDATE_ALL);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
    }
}
