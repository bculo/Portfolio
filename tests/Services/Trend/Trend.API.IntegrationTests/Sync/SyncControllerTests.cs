using System.Net;
using FluentAssertions;
using Http.Common.Extensions;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;
using Trend.Application.Interfaces.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Trend.IntegrationTests.Sync;

public static class Endpoints
{
    public const string GetSyncStatuses = "/api/v1/sync/GetSyncStatuses";
    public const string GetSync = "/api/v1/sync/GetSync";
    public const string Sync = "/api/v1/sync/Sync";
}

public class SyncControllerTests(TrendApiFactory factory) : TrendControllerBaseTest(factory)
{
    [Fact]
    public async Task Sync_ShouldReturnBadRequest_WhenNoSearchWordsAvailable()
    {
        //Arrange
        Client.WithRole(UserRole.Admin);

        //Act
        var response = await Client.GetAsync(Endpoints.Sync);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Sync_ShouldReturnNoContent_WhenSearchWordsAvailable()
    {
        //Arrange
        Client.WithRole(UserRole.Admin);
        await FixtureService.AddSearchWord();

        var engineResponse = TrendFixtureService.GenerateMockInstance<GoogleSearchEngineResponseDto>();
        Factory.MockServer
            .Given(Request.Create().WithPath("/customsearch/v1"))
            .RespondWith(
                Response.Create().WithBodyAsJson(engineResponse)
                    .WithStatusCode(HttpStatusCode.OK)
            );
        
        //Act
        var response = await Client.GetAsync(Endpoints.Sync);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task Sync_ShouldReturnBadRequest_WhenSearchEnginesDoesntWork()
    {
        //Arrange
        Client.WithRole(UserRole.Admin);
        await FixtureService.AddSearchWord();
        
        Factory.MockServer
            .Given(Request.Create().WithPath("/customsearch/v1"))
            .RespondWith(
                Response.Create().WithStatusCode(HttpStatusCode.BadRequest)
            );
        
        //Act
        var response = await Client.GetAsync(Endpoints.Sync);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task GetSync_ShouldReturnNotFound_WhenItemDoesntExists()
    {
        //Arrange
        Client.WithRole(UserRole.User);

        //Act
        var response = await Client.GetAsync($"{Endpoints.GetSync}/123123");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetSync_ShouldReturnOk_WhenValidRequest()
    {
        //Arrange
        Client.WithRole(UserRole.User);
        var existingInstance = await FixtureService.AddSyncStatus();
        
        //Act
        var response = await Client.GetAsync($"{Endpoints.GetSync}/{existingInstance.Id}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.ExtractContentFromResponse<SyncStatusResDto>();
        body.Id.Should().Be(existingInstance.Id);
    }
    
    [Fact]
    public async Task GetSyncStatuses_ShouldReturnOk_WhenValidRequest()
    {
        //Arrange
        Client.WithRole(UserRole.User);

        //Act
        var response = await Client.GetAsync(Endpoints.GetSyncStatuses);
        
        //Assert
        response.EnsureSuccessStatusCode();
    }
}