using Crypto.Application.Modules.Crypto.Commands.Delete;
using Crypto.IntegrationTests.Constants;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.CaseTests
{
    public class DeleteCryptoCaseTest : IClassFixture<CryptoApiFactory>
    {
        private readonly CryptoApiFactory _factory;

        public DeleteCryptoCaseTest(CryptoApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ExistingCryptoItemSuccessfullyDeleted()
        {
            var symbol = "BTC";

            var client = _factory.CreateClient();

            var existResponse = await client.GetAsync(ApiEndpoint.CRYPTO_FETCH_SINGLE + $"/{symbol}");
            existResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var deleteResponse = await client.DeleteAsync(ApiEndpoint.CRYPTO_DELETE + $"/{symbol}");
            deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            var shouldNotBeFoundResponse = await client.GetAsync(ApiEndpoint.CRYPTO_FETCH_SINGLE + $"/{symbol}");
            shouldNotBeFoundResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
