using Crypto.IntegrationTests.Constants;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.InfoController
{
    public class FetchInfoTests : IClassFixture<CryptoApiFactory>
    {
        private readonly HttpClient _client;

        public FetchInfoTests(CryptoApiFactory cryptoApiFactory)
        {
            _client = cryptoApiFactory.CreateClient();
        }

        [Fact]
        public async Task AppVersion_ShouldReturnAppVersionInStringFormat_WhenEndpointCalled()
        {
            var response = await _client.GetAsync(ApiEndpoint.INFO_VERSION);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var version = await response.Content.ReadAsStringAsync();
            version.Should().NotBeEmpty();
        }
    }
}
