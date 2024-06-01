using Tests.Common.Extensions;
using Tests.Common.Interfaces.Claims.Models;

namespace Trend.IntegrationTests.News;

internal static class Endpoints
{
    public const string GetLatestNews = "/api/v1/news/GetLatestNews";
    public const string GetLatestCryptoNews = "/api/v1/news/GetLatestCryptoNews";
    public const string GetLatestStockNews = "/api/v1/news/GetLatestStockNews";
}

public class NewsControllerTests(TrendApiFactory factory) : TrendControllerBaseTest(factory)
{
    [Fact]
    public async Task GetLatestNews_ShouldReturnStatusOk_WhenEndpointInvoked()
    {
        //Arrange
        Client.WithRole(UserRole.User);

        //Act
        var response = await Client.GetAsync(Endpoints.GetLatestNews);

        //Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task GetLatestStockNews_ShouldReturnStatusOk_WhenEndpointInvoked()
    {
        //Arrange
        Client.WithRole(UserRole.User);

        //Act
        var response = await Client.GetAsync(Endpoints.GetLatestStockNews);

        //Assert
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task GetLatestCryptoNews_ShouldReturnStatusOk_WhenEndpointInvoked()
    {
        //Arrange
        Client.WithRole(UserRole.User);

        //Act
        var response = await Client.GetAsync(Endpoints.GetLatestCryptoNews);

        //Assert
        response.EnsureSuccessStatusCode();
    }
}