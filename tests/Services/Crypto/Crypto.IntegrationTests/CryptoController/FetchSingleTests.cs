using Crypto.API.Filters.Models;
using Crypto.Application.Modules.Crypto.Queries.FetchSingle;
using Crypto.IntegrationTests.Common;
using Crypto.IntegrationTests.Constants;
using Crypto.IntegrationTests.Extensions;
using Crypto.IntegrationTests.Utils;
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
    public class FetchSingleTests : BaseTests
    {
        public FetchSingleTests(CryptoApiFactory factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("BTC")]
        [InlineData("ETH")]
        [InlineData("ADA")]
        public async Task FetchSingle_ShouldReturnStatusOk_WhenExistingSymbolProvided(string symbol)
        {
            //Arrange
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            var response = await _client.GetAsync($"{ApiEndpoint.CRYPTO_FETCH_SINGLE}/{symbol}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var instances = JsonConvert.DeserializeObject<FetchSingleResponseDto>(jsonResponse);
            instances.Should().BeOfType<FetchSingleResponseDto>();
            instances?.Symbol.Should().Be(symbol);
            instances?.Price.Should().BeGreaterThan(-1);
        }

        [Theory]
        [InlineData("---------")]
        [InlineData("!-!")]
        public async Task FetchSingle_ShouldReturnStatusBadRequest_WhenInvalidSymbolProvided(string symbol)
        {
            //Arrange
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            var response = await _client.GetAsync($"{ApiEndpoint.CRYPTO_FETCH_SINGLE}/{symbol}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task FetchSingle_ShouldReturnStatusNotFound_WhenNullOrEmptySymbolProvided(string symbol)
        {
            //Arrange
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            var response = await _client.GetAsync($"{ApiEndpoint.CRYPTO_FETCH_SINGLE}/{symbol}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
