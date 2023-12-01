using FluentAssertions;

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
            var client = GetAuthInstance(UserAuthType.User);

            //Act
            var response = await client.GetAsync(ApiEndpoints.GetLatestEconomyNews);

            //Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
