using FluentAssertions;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Trend.IntegrationTests.SyncController;

public class GetSyncStatusesTests : TrendControllerBaseTest
{
    public GetSyncStatusesTests(TrendApiFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task GetSyncStatuses_ShouldReturnOk_WhenValidRequest()
    {
        //Arrange
        Client.WithRole(UserRole.User);

        //Act
        var response = await Client.GetAsync(ApiEndpoints.GetSyncStatuses);
        
        //Assert
        response.EnsureSuccessStatusCode();
    }
}