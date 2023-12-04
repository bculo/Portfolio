using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trend.IntegrationTests.NewsController
{
    public class GetLastCryptoNewsBaseTest : TrendControllerBaseTest
    {
        public GetLastCryptoNewsBaseTest(TrendApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetLatestCryptoNews_ShouldReturnStatusOk_WhenEndpointInvoked()
        {
            //Arrange
            var client = GetAuthInstance(UserAuthType.User);

            //Act
            var response = await client.GetAsync(ApiEndpoints.GetLatestCryptoNews);

            //Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
