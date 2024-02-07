using FluentAssertions;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Trend.IntegrationTests.NewsController
{
    public class GetLastEconomyNewsBaseTest : TrendControllerBaseTest
    {
        public GetLastEconomyNewsBaseTest(TrendApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetLatestEconomyNews_ShouldReturnStatusOk_WhenEndpointInvoked()
        {
            //Arrange
            Client.WithRole(UserRole.User);

            //Act
            var response = await Client.GetAsync(ApiEndpoints.GetLatestEconomyNews);

            //Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
