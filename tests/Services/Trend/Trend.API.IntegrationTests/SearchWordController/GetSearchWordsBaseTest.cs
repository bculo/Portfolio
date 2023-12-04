using FluentAssertions;

namespace Trend.IntegrationTests.SearchWordController;

public class GetSearchWordsBaseTest : TrendControllerBaseTest
{
    public GetSearchWordsBaseTest(TrendApiFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task GetSearchWords_ShouldReturnStatusOk_WhenValidRequestSent()
    {
        //Arrange
        var client = GetAuthInstance(UserAuthType.User);

        //Act
        var response = await client.GetAsync(ApiEndpoints.GetSearchWords);
        
        //Assert
        response.EnsureSuccessStatusCode();
    }
}