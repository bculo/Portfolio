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
    public class UpdateTests : IClassFixture<CryptoApiFactory>
    {
        private readonly CryptoApiFactory _factory;

        public UpdateTests(CryptoApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task UpdatePrice_ShouldReturnStatusBadRequest_WhenNonexistentSymbolProvided()
        {
            string symbol = "DRAGOINCOINSS";
            var client = _factory.CreateClient();
            string requestJson = JsonConvert.SerializeObject(new { Symbol = symbol });
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ApiEndpoint.CRYPTO_UPDATE, content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdatePrice_ShouldReturnStatusNoContent_WhenExistingSymbolProvided()
        {
            string symbol = "BTC";
            var client = _factory.CreateClient();
            string requestJson = JsonConvert.SerializeObject(new { Symbol = symbol });
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ApiEndpoint.CRYPTO_UPDATE, content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

    }
}
