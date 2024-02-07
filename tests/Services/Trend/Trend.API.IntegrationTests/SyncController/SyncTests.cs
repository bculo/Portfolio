using System.Net;
using FluentAssertions;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;
using Trend.Application.Interfaces.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Trend.IntegrationTests.SyncController;

public class SyncTests : TrendControllerBaseTest
{
    public SyncTests(TrendApiFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task Sync_ShouldReturnBadRequest_WhenNoSearchWordsAvailable()
    {
        //Arrange
        Client.WithRole(UserRole.User);

        //Act
        var response = await Client.GetAsync(ApiEndpoints.Sync);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Sync_ShouldReturnNoContent_WhenSearchWordsAvailable()
    {
        //Arrange
        Client.WithRole(UserRole.User);
        await FixtureService.AddSearchWord();

        var engineResponse = TrendFixtureService.GenerateMockInstance<GoogleSearchEngineResponseDto>();
        Factory.MockServer
            .Given(Request.Create().WithPath("/customsearch/v1"))
            .RespondWith(
                Response.Create().WithBodyAsJson(engineResponse)
                    .WithStatusCode(HttpStatusCode.OK)
            );
        
        //Act
        var response = await Client.GetAsync(ApiEndpoints.Sync);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task Sync_ShouldReturnBadRequest_WhenSearchEnginesDoesntWork()
    {
        //Arrange
        Client.WithRole(UserRole.User);
        await FixtureService.AddSearchWord();
        
        Factory.MockServer
            .Given(Request.Create().WithPath("/customsearch/v1"))
            .RespondWith(
                Response.Create().WithStatusCode(HttpStatusCode.BadRequest)
            );
        
        //Act
        var response = await Client.GetAsync(ApiEndpoints.Sync);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}