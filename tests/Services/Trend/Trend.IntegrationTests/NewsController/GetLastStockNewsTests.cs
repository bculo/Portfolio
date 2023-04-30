using FluentAssertions;
using Trend.IntegrationTests.Helpers;

namespace Trend.IntegrationTests.NewsController
{
    [Collection("TrendCollection")]
    public class GetLastStockNewsTests : BaseTests
    {
        public GetLastStockNewsTests(TrendApiFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetLatestStockNews_ShouldReturnStatusOk_WhenEndpointInvoked()
        {
            //Arrange
            _client.AddJwtToken(JwtTokens.USER_ROLE_TOKEN);

            //Act
            var response = await _client.GetAsync(ApiEndpoints.LATEST_STOCK_NEWS);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
