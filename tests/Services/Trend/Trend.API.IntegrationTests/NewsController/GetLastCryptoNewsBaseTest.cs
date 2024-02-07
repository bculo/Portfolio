using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

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
            Client.WithRole(UserRole.User);

            //Act
            var response = await Client.GetAsync(ApiEndpoints.GetLatestCryptoNews);

            //Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
