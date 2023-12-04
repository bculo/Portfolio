using Dtos.Common.v1.Trend.SearchWord;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Tests.Common.Utilities;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Interfaces;

namespace Trend.IntegrationTests.SearchWordController;


public class AddNewSearchWordBaseTest : TrendControllerBaseTest
{
    public AddNewSearchWordBaseTest(TrendApiFactory factory) : base(factory)
    {
    }

    [Theory]
    [InlineData("Firstv1", (int)ContextType.Crypto, (int)SearchEngine.Google)]

    public async Task AddNewSearchWord_ShouldReturnStatusOk_WhenValidRequestObjectSent(string word, int contextType, int searchEngine)
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
        var response = await client.PostAsync(ApiEndpoints.AddNewSearchWord, request);
        
        //Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Theory]
    [InlineData("", (int)ContextType.Crypto, (int)SearchEngine.Google)]
    [InlineData("-", (int)ContextType.Economy, (int)SearchEngine.Google)]
    [InlineData("FirstFial", 800, (int)SearchEngine.Google)]
    [InlineData("SecondFail", (int)ContextType.Economy, 100)]
    public async Task AddNewSearchWord_ShouldReturnStatusBadRequest_WhenInvalidRequestObjectSent(string word, int contextType, int searchEngine)
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
        var response = await client.PostAsync(ApiEndpoints.AddNewSearchWord, request);
        
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("BTC", (int)ContextType.Crypto, (int)SearchEngine.Google)]
    [InlineData("ETH", (int)ContextType.Crypto, (int)SearchEngine.Google)]
    public async Task AddNewSearchWord_ShouldReturnStatusBadRequest_WhenWordWithSameEngineAlreadyExists(string word, int contextType, int searchEngine)
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

        await _fixtureService.AddSearchWord(new SearchWord
        {
            Word = word,
            Type = (ContextType)contextType,
            Engine = (SearchEngine)searchEngine
        });
        
        //Actx
        var response = await client.PostAsync(ApiEndpoints.AddNewSearchWord, request);
        
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}