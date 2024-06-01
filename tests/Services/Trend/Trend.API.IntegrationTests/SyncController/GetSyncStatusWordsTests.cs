using System.Net;
using FluentAssertions;
using Http.Common.Extensions;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;
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
        Client.WithRole(UserRole.User);

        //Act
        var response = await Client.GetAsync($"{ApiEndpoints.GetSyncStatusWords}/{id}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetSyncStatusWords_ShouldReturnStatusOk_WhenInstanceExists()
    {
        //Arrange
        Client.WithRole(UserRole.User);
        var existingInstance = await FixtureService.AddSyncStatus();
        
        //Act
        var response = await Client.GetAsync($"{ApiEndpoints.GetSyncStatusWords}/{existingInstance.Id}");
        
        //Assert
        response.EnsureSuccessStatusCode();
        var body = await response.ExtractContentFromResponse<List<SyncSearchWordResDto>>();
        body.Count.Should().Be(existingInstance.UsedSyncWords.Count);
    }
}