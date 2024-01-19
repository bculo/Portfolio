using System.Net;
using FluentAssertions;
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
        var client = GetAuthInstance(UserAuthType.User);

        //Act
        var response = await client.GetAsync(ApiEndpoints.Sync);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task Sync_ShouldReturnNoContent_WhenSearchWordsAvailable()
    {
        //Arrange
        var client = GetAuthInstance(UserAuthType.User);
        await _fixtureService.AddSearchWord();

        var engineResponse = TrendFixtureService.GenerateMockInstance<GoogleSearchEngineResponseDto>();
        _factory.MockServer
            .Given(Request.Create().WithPath("/customsearch/v1"))
            .RespondWith(
                Response.Create().WithBodyAsJson(engineResponse)
                    .WithStatusCode(HttpStatusCode.OK)
            );
        
        //Act
        var response = await client.GetAsync(ApiEndpoints.Sync);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task Sync_ShouldReturnBadRequest_WhenSearchEnginesDoesntWork()
    {
        //Arrange
        var client = GetAuthInstance(UserAuthType.User);
        await _fixtureService.AddSearchWord();
        
        _factory.MockServer
            .Given(Request.Create().WithPath("/customsearch/v1"))
            .RespondWith(
                Response.Create().WithStatusCode(HttpStatusCode.BadRequest)
            );
        
        //Act
        var response = await client.GetAsync(ApiEndpoints.Sync);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}