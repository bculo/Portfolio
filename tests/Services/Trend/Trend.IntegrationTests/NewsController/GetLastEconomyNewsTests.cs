using FluentAssertions;

namespace Trend.IntegrationTests.NewsController
{
    public class GetLastEconomyNewsTests : BaseTests
    {
        public GetLastEconomyNewsTests(TrendApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetLatestEconomyNews_ShouldReturnStatusOk_WhenEndpointInvoked()
        {
            //Arrange
            var client = GetAuthInstance(UserAuthType.User);

            //Act
            var response = await client.GetAsync(ApiEndpoints.LATEST_ECONOMY_NEWS);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
