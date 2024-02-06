using FluentAssertions;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

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
        Client.AsUserRole(UserRole.User);

        //Act
        var response = await Client.GetAsync(ApiEndpoints.GetSearchWords);
        
        //Assert
        response.EnsureSuccessStatusCode();
    }
}