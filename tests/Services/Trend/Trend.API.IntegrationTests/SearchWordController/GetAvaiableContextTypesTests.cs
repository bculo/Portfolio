using FluentAssertions;

namespace Trend.IntegrationTests.SearchWordController;

public class GetAvailableContextTypesBaseTest : TrendControllerBaseTest
{
    public GetAvailableContextTypesBaseTest(TrendApiFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task GetAvailableContextTypes_ShouldReturnStatusOk_WhenValidRequestSent()
    {
        //Arrange
        var client = GetAuthInstance(UserAuthType.User);

        //Act
        var response = await client.GetAsync(ApiEndpoints.GetAvailableContextTypes);
        
        //Assert
        response.EnsureSuccessStatusCode();
    }
}