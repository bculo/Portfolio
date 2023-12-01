using FluentAssertions;

namespace Trend.IntegrationTests.SearchWordController;

public class GetAvaiableSearchEnginesBaseTest : TrendControllerBaseTest
{
    
    public GetAvaiableSearchEnginesBaseTest(TrendApiFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task GetAvaiableSearchEngines_ShouldReturnStatusOk_WhenValidRequestSent()
    {
        //Arrange
        var client = GetAuthInstance(UserAuthType.User);

        //Act
        var response = await client.GetAsync(ApiEndpoints.GetAvailableSearchEngines);
        
        //Assert
        response.EnsureSuccessStatusCode();
    }
}