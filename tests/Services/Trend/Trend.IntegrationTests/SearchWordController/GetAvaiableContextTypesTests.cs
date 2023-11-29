using FluentAssertions;

namespace Trend.IntegrationTests.SearchWordController;

public class GetAvailableContextTypesTests : TrendControllerTests
{
    public GetAvailableContextTypesTests(TrendApiFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task GetAvailableContextTypes_ShouldReturnStatusOk_WhenValidRequestSent()
    {
        //Arrange
        var client = GetAuthInstance(UserAuthType.User);

        //Act
        var response = await client.GetAsync(ApiEndpoints.AVAILABLE_CONTEXT_TYPES);
        
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}