using Crypto.IntegrationTests.Constants;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.CryptoController
{
    public class GetPriceHistoryTests : IClassFixture<CryptoApiFactory>
    {
        private readonly CryptoApiFactory _factory;

        public GetPriceHistoryTests(CryptoApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetPriceHistory_ShouldReturnStatusOk_WhenExistingSymbolProvided()
        {
            string symbol = "BTC";
            var client = _factory.CreateClient();

            var response = await client.GetAsync(ApiEndpoint.CRYPTO_PRICE_HISTORY + $"/{symbol}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPriceHistory_ShouldReturnBadRequest_WhenInvalidSymbolProvided()
        {
            string symbol = "DRAGONTON";
            var client = _factory.CreateClient();

            var response = await client.GetAsync(ApiEndpoint.CRYPTO_PRICE_HISTORY + $"/{symbol}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetPriceHistory_ShouldReturnNotFound_WhenNullSymbolProvided()
        {
            string? symbol = null;
            var client = _factory.CreateClient();

            var response = await client.GetAsync(ApiEndpoint.CRYPTO_PRICE_HISTORY + $"/{symbol}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
