using FluentAssertions;

namespace Trend.IntegrationTests.SearchWordController;

[Collection("TrendCollection")]
public class GetAvaiableContextTypesTests : BaseTests
{
    public GetAvaiableContextTypesTests(TrendApiFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task GetAvaiableContextTypes_ShouldReturnStatusOk_WhenValidRequestSent()
    {
        //Arrange
        var client = GetAuthInstance(UserAuthType.User);

        //Act
        var response = await client.GetAsync(ApiEndpoints.AVAILABLE_CONTEXT_TYPES);
        
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}