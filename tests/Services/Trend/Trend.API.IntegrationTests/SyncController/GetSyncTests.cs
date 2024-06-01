using System.Net;
using FluentAssertions;
using Http.Common.Extensions;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;
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
        Client.WithRole(UserRole.User);

        //Act
        var response = await Client.GetAsync($"{ApiEndpoints.GetSync}/123123");
        
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
        var response = await Client.GetAsync($"{ApiEndpoints.GetSync}/{existingInstance.Id}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.ExtractContentFromResponse<SyncStatusResDto>();
        body.Id.Should().Be(existingInstance.Id);
    }
}