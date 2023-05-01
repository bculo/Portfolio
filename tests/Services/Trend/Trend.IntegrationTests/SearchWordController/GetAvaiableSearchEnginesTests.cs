using FluentAssertions;

namespace Trend.IntegrationTests.SearchWordController;

[Collection("TrendCollection")]
public class GetAvaiableSearchEnginesTests : BaseTests
{
    
    public GetAvaiableSearchEnginesTests(TrendApiFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task GetAvaiableSearchEngines_ShouldReturnStatusOk_WhenValidRequestSent()
    {
        //Arrange
        var client = GetAuthInstance(UserAuthType.User);

        //Act
        var response = await client.GetAsync(ApiEndpoints.AVAILABLE_ENGINES_TYPES);
        
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}