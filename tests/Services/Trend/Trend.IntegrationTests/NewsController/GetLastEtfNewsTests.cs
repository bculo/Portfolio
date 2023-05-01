using FluentAssertions;

namespace Trend.IntegrationTests.NewsController
{
    [Collection("TrendCollection")]
    public class GetLastEtfNewsTests : BaseTests
    {
        public GetLastEtfNewsTests(TrendApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetLatestEtfsNews_ShouldReturnStatusOk_WhenEndpointInvoked()
        {
            //Arrange
            var client = GetAuthInstance(UserAuthType.User);

            //Act
            var response = await client.GetAsync(ApiEndpoints.LATEST_ETF_NEWS);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
