using FluentAssertions;

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
            var client = GetAuthInstance(UserAuthType.User);

            //Act
            var response = await client.GetAsync(ApiEndpoints.GetLatestEtfNews);

            //Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
