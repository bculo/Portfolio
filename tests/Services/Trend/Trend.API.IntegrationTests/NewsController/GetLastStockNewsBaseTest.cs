using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Trend.IntegrationTests.NewsController
{

    public class GetLastStockNewsBaseTest : TrendControllerBaseTest
    {
        public GetLastStockNewsBaseTest(TrendApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetLatestStockNews_ShouldReturnStatusOk_WhenEndpointInvoked()
        {
            //Arrange
            Client.AsUserRole(UserRole.User);

            //Act
            var response = await Client.GetAsync(ApiEndpoints.GetLatestStockNews);

            //Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
