using FluentAssertions;

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
        var client = GetAuthInstance(UserAuthType.User);

        //Act
        var response = await client.GetAsync(ApiEndpoints.GetSyncStatuses);
        
        //Assert
        response.EnsureSuccessStatusCode();
    }
}