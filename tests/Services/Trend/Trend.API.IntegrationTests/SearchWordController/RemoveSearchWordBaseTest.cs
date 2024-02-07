using System.Net;
using FluentAssertions;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Trend.IntegrationTests.SearchWordController;

public class RemoveSearchWordBaseTest : TrendControllerBaseTest
{
    public RemoveSearchWordBaseTest(TrendApiFactory factory) : base(factory)
    {
    }
    
    [Theory]
    [InlineData("BRB")]
    [InlineData("ASD")]
    public async Task RemoveSearchWord_ShouldReturnStatusBadRequest_WhenWordWithGivenIdDoesntExist(string id)
    {
        //Arrange
        Client.WithRole(UserRole.Admin);

        //Act
        var response = await Client.DeleteAsync($"{ApiEndpoints.RemoveSearchWord}/{id}");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData("645188e6207b70747e47aea2")]
    public async Task RemoveSearchWord_ShouldReturnStatusOk_WhenWordWithGivenIdExist(string id)
    {
        //Arrange
        Client.WithRole(UserRole.Admin);
        
        await FixtureService.AddSearchWord(id: id);

        var httpContent = "".AsHttpContent();
        
        //Act
        var response = await Client.PutAsync($"{ApiEndpoints.RemoveSearchWord}/{id}", httpContent);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}