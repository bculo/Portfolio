using FluentAssertions;
using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.IntegrationTests.SearchWordController;

public class RemoveSearchWordTests : TrendControllerTests
{
    public RemoveSearchWordTests(TrendApiFactory factory) : base(factory)
    {
    }
    
    [Theory]
    [InlineData("BRB")]
    [InlineData("ASD")]
    public async Task RemoveSearchWord_ShouldReturnStatusBadRequest_WhenWordWithGivenIdDoesntExist(string id)
    {
        //Arrange
        var client = GetAuthInstance(UserAuthType.User);

        //Act
        var response = await client.DeleteAsync($"{ApiEndpoints.REMOVE_SEARCH_WORD}/{id}");
        
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData("645188e6207b70747e47aea2")]
    public async Task RemoveSearchWord_ShouldReturnStatusOk_WhenWordWithGivenIdExist(string id)
    {
        //Arrange
        var client = GetAuthInstance(UserAuthType.User);

        await _fixtureService.AddSearchWord(new SearchWord
        {
            Id = id,
            Word = "Word",
            Engine = SearchEngine.Google,
            Type = ContextType.Crypto
        });
        
        //Act
        var response = await client.DeleteAsync($"{ApiEndpoints.REMOVE_SEARCH_WORD}/{id}");
        
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }
}