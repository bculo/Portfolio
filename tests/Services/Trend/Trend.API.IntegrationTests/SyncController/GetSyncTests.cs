using System.Net;
using FluentAssertions;
using Http.Common.Extensions;
using Trend.Application.Interfaces.Models;

namespace Trend.IntegrationTests.SyncController;

public class GetSyncTests : TrendControllerBaseTest
{
    public GetSyncTests(TrendApiFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task GetSync_ShouldReturnNotFound_WhenItemDoesntExists()
    {
        //Arrange
        var client = GetAuthInstance(UserAuthType.User);

        //Act
        var response = await client.GetAsync($"{ApiEndpoints.GetSync}/123123");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetSync_ShouldReturnOk_WhenValidRequest()
    {
        //Arrange
        var client = GetAuthInstance(UserAuthType.User);
        var existingInstance = await _fixtureService.AddSyncStatus();
        
        //Act
        var response = await client.GetAsync($"{ApiEndpoints.GetSync}/{existingInstance.Id}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.HandleResponse<SyncStatusResDto>();
        body.Id.Should().Be(existingInstance.Id);
    }
}