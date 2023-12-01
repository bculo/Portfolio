using FluentAssertions;

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
            var client = GetAuthInstance(UserAuthType.User);

            //Act
            var response = await client.GetAsync(ApiEndpoints.GetLatestStockNews);

            //Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
