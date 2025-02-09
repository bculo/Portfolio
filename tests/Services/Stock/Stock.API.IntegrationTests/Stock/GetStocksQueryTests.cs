using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Stock.API.IntegrationTests.Stock;

public class GetStocksQueryTests : StockControllerBaseTest
{
    public GetStocksQueryTests(StockApiFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task ShouldReturnStatusOk_WhenEndpointInvoked()
    {
        //Arrange
        Client.WithRole(UserRole.User);

        //Act
        var response = await Client.GetAsync("/api/v1/stock/all");

        //Assert
        response.EnsureSuccessStatusCode();
    }
}