using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.IntegrationTests.NewsController
{
    [Collection("TrendCollection")]
    public class GetLastCryptoNewsTests : BaseTests
    {
        public GetLastCryptoNewsTests(TrendApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetLatestCryptoNews_ShouldReturnStatusOk_WhenEndpointInvoked()
        {
            //Arrange
            var client = GetAuthInstance(UserAuthType.User);

            //Act
            var response = await client.GetAsync(ApiEndpoints.LATEST_CRYPTO_NEWS);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
