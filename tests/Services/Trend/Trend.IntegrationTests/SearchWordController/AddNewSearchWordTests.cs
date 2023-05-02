using Dtos.Common.v1.Trend.SearchWord;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Tests.Common.Utilities;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Interfaces;

namespace Trend.IntegrationTests.SearchWordController;


public class AddNewSearchWordTests : BaseTests
{
    public AddNewSearchWordTests(TrendApiFactory factory) : base(factory)
    {
    }

    [Theory]
    [InlineData("Firstv1", (int)ContextType.Crypto, (int)SearchEngine.Google)]
    [InlineData("Secondv2", (int)ContextType.Economy, (int)SearchEngine.Google)]
    [InlineData("Thirdv3", (int)ContextType.Etf, (int)SearchEngine.Google)]
    [InlineData("Fourthv4", (int)ContextType.Stock, (int)SearchEngine.Google)]
    public async Task AddNewSearchWord_ShouldReturnStatusOk_WhenValidRequestSent(string word, int contextType, int searchEngine)
    {
        //Arrange
        var instance = new SearchWordCreateDto
        {
            SearchWord = word,
            ContextType = contextType,
            SearchEngine = searchEngine
        };
        var request = HttpClientUtilities.PrepareJsonRequest(instance);
        var client = GetAuthInstance(UserAuthType.User);

        //Act
        var response = await client.PostAsync(ApiEndpoints.ADD_NEW_SEARH_WORD, request);
        
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
    
    [Theory]
    [InlineData("", (int)ContextType.Crypto, (int)SearchEngine.Google)]
    [InlineData("-", (int)ContextType.Economy, (int)SearchEngine.Google)]
    [InlineData("FirstFial", 800, (int)SearchEngine.Google)]
    [InlineData("SecondFail", (int)ContextType.Economy, 100)]
    public async Task AddNewSearchWord_ShouldReturnStatusBadRequest_WhenInvalidRequestSent(string word, int contextType, int searchEngine)
    {
        //Arrange
        var instance = new SearchWordCreateDto
        {
            SearchWord = word,
            ContextType = contextType,
            SearchEngine = searchEngine
        };
        var request = HttpClientUtilities.PrepareJsonRequest(instance);
        var client = GetAuthInstance(UserAuthType.User);

        //Act
        var response = await client.PostAsync(ApiEndpoints.ADD_NEW_SEARH_WORD, request);
        
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddNewSearchWord_ShouldReturnStatusBadRequest_WhenWordWithSameEngineAlreadyExists()
    {
        //Arrange
        var instance = new SearchWordCreateDto
        {
            SearchWord = MockConstants.EXISTING_SEARCH_WORD_TEXT,
            ContextType = (int)ContextType.Crypto,
            SearchEngine = (int)SearchEngine.Google
        };
        var request = HttpClientUtilities.PrepareJsonRequest(instance);
        var client = GetAuthInstance(UserAuthType.User);

        //Act
        var response = await client.PostAsync(ApiEndpoints.ADD_NEW_SEARH_WORD, request);
        
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}