using FluentAssertions;

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
            var client = GetAuthInstance(UserAuthType.User);

            //Act
            var response = await client.GetAsync(ApiEndpoints.GetLatestNews);

            //Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
