using Crypto.API.Filters.Models;
using Crypto.Application.Modules.Crypto.Queries.FetchSingle;
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
    [Collection("CryptoCollection")]
    public class FetchSingleTests
    {
        private readonly CryptoApiFactory _factory;

        public FetchSingleTests(CryptoApiFactory factory)
        {
            _factory = factory;
        }

        // [Fact]
        public async Task FetchSingle_ShouldReturnStatusOk_WhenExistingSymbolProvided()
        {
            string symbol = "BTC";
            var client = _factory.CreateClient();

            var response = await client.GetAsync(ApiEndpoint.CRYPTO_FETCH_SINGLE + $"/{symbol}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var instances = JsonConvert.DeserializeObject<FetchSingleResponseDto>(jsonResponse);
            instances.Should().BeOfType<FetchSingleResponseDto>();
        }

        //[Fact]
        public async Task FetchSingle_ShouldReturnBadRequest_WhenNonexistentSymbolProvided()
        {
            string symbol = "DRAGONTON";
            var client = _factory.CreateClient();

            var response = await client.GetAsync(ApiEndpoint.CRYPTO_FETCH_SINGLE + $"/{symbol}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        //[Fact]
        public async Task GetPriceHistory_ShouldReturnBadRequest_WhenInvalidSymbolFormatProvided()
        {
            string symbol = "SYMOL12";
            var client = _factory.CreateClient();

            var response = await client.GetAsync(ApiEndpoint.CRYPTO_FETCH_SINGLE + $"/{symbol}");
            
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ValidationErrorResponse>(jsonResponse);
            error.Should().NotBeNull();
            error!.Message.Should().Be("Validation exception");
            error!.Errors.Should().NotBeEmpty();
        }

        //[Fact]
        public async Task FetchSingle_ShouldReturnNotFound_WhenNullSymbolProvided()
        {
            string? symbol = null;
            var client = _factory.CreateClient();

            var response = await client.GetAsync(ApiEndpoint.CRYPTO_FETCH_SINGLE + $"/{symbol}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
