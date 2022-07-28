using Crypto.IntegrationTests.Constants;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.CryptoController
{
    public class FetchSingleTests : IClassFixture<CryptoApiFactory>
    {
        private readonly CryptoApiFactory _factory;

        public FetchSingleTests(CryptoApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task FetchSingle_ShouldReturnStatusOk_WhenExistingSymbolProvided()
        {
            string symbol = "BTC";
            var client = _factory.CreateClient();

            var response = await client.GetAsync(ApiEndpoint.CRYPTO_FETCH_SINGLE + $"/{symbol}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task FetchSingle_ShouldReturnBadRequest_WhenInvalidSymbolProvided()
        {
            string symbol = "DRAGONTON";
            var client = _factory.CreateClient();

            var response = await client.GetAsync(ApiEndpoint.CRYPTO_FETCH_SINGLE + $"/{symbol}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task FetchSingle_ShouldReturnNotFound_WhenNullSymbolProvided()
        {
            string? symbol = null;
            var client = _factory.CreateClient();

            var response = await client.GetAsync(ApiEndpoint.CRYPTO_FETCH_SINGLE + $"/{symbol}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
