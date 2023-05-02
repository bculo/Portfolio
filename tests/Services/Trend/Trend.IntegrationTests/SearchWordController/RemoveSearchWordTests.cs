using FluentAssertions;

namespace Trend.IntegrationTests.SearchWordController;

public class RemoveSearchWordTests : BaseTests
{
    public RemoveSearchWordTests(TrendApiFactory factory) : base(factory)
    {
    }
    
    [Theory]
    [InlineData("645188e6207b70747e47aea2")]
    [InlineData("645188f7f6a006554af044d6")]
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
    [InlineData(MockConstants.EXISTING_SEARCH_WORD_ID)]
    public async Task RemoveSearchWord_ShouldReturnStatusOk_WhenWordWithGivenIdExist(string id)
    {
        //Arrange
        var client = GetAuthInstance(UserAuthType.User);

        //Act
        var response = await client.DeleteAsync($"{ApiEndpoints.REMOVE_SEARCH_WORD}/{id}");
        
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }
}