using FluentAssertions;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

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
        Client.WithRole(UserRole.User);

        //Act
        var response = await Client.GetAsync(ApiEndpoints.GetAvailableContextTypes);
        
        //Assert
        response.EnsureSuccessStatusCode();
    }
}