using FluentAssertions;

namespace Trend.IntegrationTests.SearchWordController;

[Collection("TrendCollection")]
public class GetSearchWordsTests : BaseTests
{
    public GetSearchWordsTests(TrendApiFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task GetSearchWords_ShouldReturnStatusOk_WhenValidRequestSent()
    {
        //Arrange
        var client = GetAuthInstance(UserAuthType.User);

        //Act
        var response = await client.GetAsync(ApiEndpoints.AVAILABLE_SEARCH_WORDS);
        
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}