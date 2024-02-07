using FluentAssertions;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Trend.IntegrationTests.NewsController
{

    public class GetLastEtfNewsBaseTest : TrendControllerBaseTest
    {
        public GetLastEtfNewsBaseTest(TrendApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetLatestEtfsNews_ShouldReturnStatusOk_WhenEndpointInvoked()
        {
            //Arrange
            Client.WithRole(UserRole.User);

            //Act
            var response = await Client.GetAsync(ApiEndpoints.GetLatestEtfNews);

            //Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
