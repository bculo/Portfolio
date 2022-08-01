using Crypto.Application.Modules.Crypto.Commands.AddNew;
using Crypto.Application.Modules.Crypto.Queries.FetchSingle;
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
    public class AddCryptoCaseTest : IClassFixture<CryptoApiFactory>
    {
        private readonly CryptoApiFactory _factory;

        public AddCryptoCaseTest(CryptoApiFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task NewNonExistentValidCryptoItemSuccessfullyPersissted()
        {
            var newSymbol = "TFUEL";

            var client = _factory.CreateClient();

            var existResponse = await client.GetAsync(ApiEndpoint.CRYPTO_FETCH_SINGLE + $"/{newSymbol}");
            existResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            var addRequest = new AddNewCommand { Symbol = newSymbol };
            var requestJson = new StringContent(JsonConvert.SerializeObject(addRequest), Encoding.UTF8, "application/json");
            var addResponse = await client.PostAsync(ApiEndpoint.CRYPTO_ADD_NEW, requestJson);
            addResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            var shouldExistResponse = await client.GetAsync(ApiEndpoint.CRYPTO_FETCH_SINGLE + $"/{newSymbol}");
            shouldExistResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var jsonResponse = await shouldExistResponse.Content.ReadAsStringAsync();
            var instances = JsonConvert.DeserializeObject<FetchSingleResponseDto>(jsonResponse);
            instances.Should().BeOfType<FetchSingleResponseDto>();
            instances!.Symbol.Should().Be(newSymbol);
        }

        [Fact]
        public async Task NewNonExistentInvalidCryptoNotSuccessfullyPersisted()
        {
            var newSymbol = "TFUEL22";

            var client = _factory.CreateClient();

            var addRequest = new AddNewCommand { Symbol = newSymbol };
            var requestJson = new StringContent(JsonConvert.SerializeObject(addRequest), Encoding.UTF8, "application/json");
            var addResponse = await client.PostAsync(ApiEndpoint.CRYPTO_ADD_NEW, requestJson);
            addResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            var existsResponse = await client.GetAsync(ApiEndpoint.CRYPTO_FETCH_SINGLE + $"/{newSymbol}");
            existsResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
