using Dtos.Common.v1.Trend.Sync;
using FluentAssertions;
using Http.Common.Extensions;

namespace Trend.IntegrationTests.SyncController;

public class GetSyncStatusWordsTests : TrendControllerBaseTest
{
    public GetSyncStatusWordsTests(TrendApiFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task GetSyncStatusWords_ShouldReturnStatusNotFound_WhenInstanceDoesntExists()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var client = GetAuthInstance(UserAuthType.User);

        //Act
        var response = await client.GetAsync($"{ApiEndpoints.GetSyncStatusWords}/{id}");
        
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetSyncStatusWords_ShouldReturnStatusOk_WhenInstanceExists()
    {
        //Arrange
        var client = GetAuthInstance(UserAuthType.User);
        var existingInstance = await _fixtureService.AddSyncStatus();
        
        //Act
        var response = await client.GetAsync($"{ApiEndpoints.GetSyncStatusWords}/{existingInstance.Id}");
        
        //Assert
        response.EnsureSuccessStatusCode();
        var body = await response.HandleResponse<List<SyncStatusWordDto>>();
        body.Count.Should().Be(existingInstance.UsedSyncWords.Count);
    }
}