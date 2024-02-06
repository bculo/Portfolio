using FluentAssertions;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Trend.IntegrationTests.NewsController
{

    public class GetLatestNewsBaseTest : TrendControllerBaseTest
    {
        public GetLatestNewsBaseTest(TrendApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetLatestNews_ShouldReturnStatusOk_WhenEndpointInvoked()
        {
            //Arrange
            Client.AsUserRole(UserRole.User);

            //Act
            var response = await Client.GetAsync(ApiEndpoints.GetLatestNews);

            //Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
