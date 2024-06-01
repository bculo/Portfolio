using FluentAssertions;
using Http.Common.Extensions;
using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Trend.IntegrationTests.Dictionary;

internal static class Endpoints
{
    public const string GetDefaultAllValue = "/api/v1/dictionary/GetDefaultAllValue";
    public const string GetSearchEngines = "/api/v1/dictionary/GetSearchEngines";
    public const string GetContextTypes = "/api/v1/dictionary/GetContextTypes";
    public const string GetActiveFilterOptions = "/api/v1/dictionary/GetActiveFilterOptions";
    public const string GetSortFilterOptions = "/api/v1/dictionary/GetSortFilterOptions";
}

public class DictionaryControllerTests(TrendApiFactory factory) : TrendControllerBaseTest(factory)
{
    [Fact]
    public async Task GetDefaultAllValue_ShouldReturnStatusOk_WhenEndpointInvoked()
    {
        //Arrange
        Client.WithRole(UserRole.User);

        //Act
        var response = await Client.GetAsync(Endpoints.GetDefaultAllValue);

        //Assert
        response.EnsureSuccessStatusCode();
        var body = await response.ExtractContentFromResponse<int>();
        body.Should().BeOfType(typeof(int));
    }
    
    [Fact]
    public async Task GetSearchEngines_ShouldReturnStatusOk_WhenEndpointInvoked()
    {
        //Arrange
        Client.WithRole(UserRole.User);

        //Act
        var response = await Client.GetAsync(Endpoints.GetSearchEngines);

        //Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task GetContextTypes_ShouldReturnStatusOk_WhenEndpointInvoked()
    {
        //Arrange
        Client.WithRole(UserRole.User);

        //Act
        var response = await Client.GetAsync(Endpoints.GetContextTypes);

        //Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task GetActiveFilterOptions_ShouldReturnStatusOk_WhenEndpointInvoked()
    {
        //Arrange
        Client.WithRole(UserRole.User);

        //Act
        var response = await Client.GetAsync(Endpoints.GetActiveFilterOptions);

        //Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task GetSortFilterOptions_ShouldReturnStatusOk_WhenEndpointInvoked()
    {
        //Arrange
        Client.WithRole(UserRole.User);

        //Act
        var response = await Client.GetAsync(Endpoints.GetSortFilterOptions);

        //Assert
        response.EnsureSuccessStatusCode();
    }
}