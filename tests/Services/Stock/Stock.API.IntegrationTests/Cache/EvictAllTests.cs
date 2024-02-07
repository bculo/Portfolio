using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Stock.API.IntegrationTests.Cache;

public class EvictAllTests : StockControllerBaseTest
{
    public EvictAllTests(StockApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task ShouldReturnSuccessCode_WhenInvokedSuccessfully()
    {
        //Arrange
        Client.WithRole(UserRole.Admin);

        //Act
        var response = await Client.DeleteAsync("/api/v1/cache/evictall");

        //Assert
        response.EnsureSuccessStatusCode();
    }
}