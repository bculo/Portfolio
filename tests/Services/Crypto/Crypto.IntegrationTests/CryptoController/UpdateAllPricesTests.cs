using Crypto.IntegrationTests.Common;
using Crypto.IntegrationTests.Constants;
using Crypto.IntegrationTests.Extensions;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common.Extensions;

namespace Crypto.IntegrationTests.CryptoController
{
    public class UpdateAllPricesTests : BaseTests
    {
        public UpdateAllPricesTests(CryptoApiFactory factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task GetAsync_ShouldReturnStatusNoContent_WhenMethodCalled()
        {
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            var response = await _client.GetAsync(ApiEndpoint.CRYPTO_UPDATE_PRICE_ALL);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }
    }
}
