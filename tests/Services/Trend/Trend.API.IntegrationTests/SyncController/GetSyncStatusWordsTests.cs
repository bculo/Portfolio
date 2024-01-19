using System.Net;
using FluentAssertions;
using Http.Common.Extensions;
using Trend.Application.Interfaces.Models;

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
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
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
        var body = await response.HandleResponse<List<SyncStatusWordResDto>>();
        body.Count.Should().Be(existingInstance.UsedSyncWords.Count);
    }
}