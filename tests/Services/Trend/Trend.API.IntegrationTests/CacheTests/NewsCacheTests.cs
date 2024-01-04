using System.Net;
using Dtos.Common.v1.Trend.Article;
using FluentAssertions;
using Http.Common.Extensions;
using Trend.Application.Interfaces.Models.Services.Google;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Trend.IntegrationTests.CacheTests;

public class NewsCacheTests: TrendControllerBaseTest
{
    public NewsCacheTests(TrendApiFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task NewsCache_ShouldReturnItem_WhenCacheInvalidated()
    {
        var client = GetAuthInstance(UserAuthType.User);
        var responseOne = await client.GetAsync(NewsController.ApiEndpoints.GetLatestNews);
        responseOne.EnsureSuccessStatusCode();
        var bodyOne = await responseOne.HandleResponse<List<ArticleDto>>();
        bodyOne.Should().BeEmpty();

        await _fixtureService.AddSearchWord();
        var engineResponse = TrendFixtureService.GenerateMockInstance<GoogleSearchEngineResponseDto>();
        _factory.MockServer
            .Given(Request.Create().WithPath("/customsearch/v1"))
            .RespondWith(
                Response.Create().WithBodyAsJson(engineResponse)
                    .WithStatusCode(HttpStatusCode.OK)
            );
        var responseTwo = await client.GetAsync(SyncController.ApiEndpoints.Sync);
        responseTwo.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        var responseThree = await client.GetAsync(NewsController.ApiEndpoints.GetLatestNews);
        var bodyThree = await responseThree.HandleResponse<List<ArticleDto>>();
        bodyThree.Should().NotBeEmpty();
    }
}