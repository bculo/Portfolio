using Crypto.IntegrationTests.Constants;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.CryptoController
{
    public class AddNewTests : IClassFixture<CryptoApiFactory>
    {
        private readonly CryptoApiFactory _factory;

        public AddNewTests(CryptoApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AddNewCrypto_ShouldReturnStatusBadRequest_WhenSymbolAlreadyExists()
        {
            string symbol = "BTC";
            var client = _factory.CreateClient();
            string requestJson = JsonConvert.SerializeObject(new { Symbol = symbol });
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ApiEndpoint.CRYPTO_ADD_NEW, content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task AddNewCrypto_ShouldReturnStatusBadRequest_WhenSymbolHasInvalidFormat()
        {
            //Symbol should not contain numbers
            string symbol = "BTC2";
            var client = _factory.CreateClient();
            string requestJson = JsonConvert.SerializeObject(new { Symbol = symbol });
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ApiEndpoint.CRYPTO_ADD_NEW, content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddNewCrypto_ShouldReturnStatusNoContent_WhenNonexistentValidSymbolProvided()
        {
            string symbol = "MATIC";
            var client = _factory.CreateClient();
            string requestJson = JsonConvert.SerializeObject(new { Symbol = symbol });
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ApiEndpoint.CRYPTO_ADD_NEW, content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
    }
}
