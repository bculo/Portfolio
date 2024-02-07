using FluentAssertions;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

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
        Client.WithRole(UserRole.User);

        //Act
        var response = await Client.GetAsync(ApiEndpoints.GetAvailableSearchEngines);
        
        //Assert
        response.EnsureSuccessStatusCode();
    }
}