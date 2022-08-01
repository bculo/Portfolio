using Crypto.IntegrationTests.Constants;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.CryptoController
{
    public class DeleteTests : IClassFixture<CryptoApiFactory>
    {
        private readonly CryptoApiFactory _factory;

        public DeleteTests(CryptoApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Delete_ShouldReturnStatusNoContent_WhenExistingSymbolProvided()
        {
            string symbol = "BTC";
            var client = _factory.CreateClient();

            var response = await client.DeleteAsync(ApiEndpoint.CRYPTO_DELETE + $"/{symbol}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_ShouldReturnBadRequest_WhenNonexistentSymbolProvided()
        {
            string symbol = "DRAGONTON";
            var client = _factory.CreateClient();

            var response = await client.DeleteAsync(ApiEndpoint.CRYPTO_DELETE + $"/{symbol}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenNullSymbolProvided()
        {
            string? symbol = null;
            var client = _factory.CreateClient();

            var response = await client.DeleteAsync(ApiEndpoint.CRYPTO_DELETE + $"/{symbol}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
