using AutoFixture;
using AutoMapper;
using Dtos.Common.v1.Trend.Article;
using FluentAssertions;
using NSubstitute;
using Polly;
using Trend.Application.Services;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Interfaces;

namespace Trend.UnitTests.Application;

public class ArticleServiceTests
{
    private readonly Fixture _fixture = new();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IArticleRepository _artRepo = Substitute.For<IArticleRepository>();
    
    [Fact]
    public async Task GetLatestNewsByContextType_ShouldReturnDtoList_WhenMethodInvoked()
    {
        var articles = _fixture.CreateMany<Article>().ToList();
        var articleDtos = _fixture.CreateMany<ArticleDto>(5).ToList();

        _artRepo.GetActiveArticles(Arg.Any<ContextType>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(articles));
        _mapper.Map<List<ArticleDto>>(articles).Returns(articleDtos);

        var articleService = new ArticleService(_artRepo, _mapper);
        
        var result = await articleService.GetLatestNewsByContextType(ContextType.Crypto, default);

        result.Count.Should().Be(5);
    }
    
    [Fact]
    public async Task GetLatestNews_ShouldReturnDtoList_WhenMethodInvoked()
    {
        var articles = _fixture.CreateMany<Article>().ToList();
        var articleDtos = _fixture.CreateMany<ArticleTypeDto>(5).ToList();

        _artRepo.GetActiveArticles(Arg.Any<CancellationToken>()).Returns(Task.FromResult(articles));
        _mapper.Map<List<ArticleTypeDto>>(articles).Returns(articleDtos);

        var articleService = new ArticleService(_artRepo, _mapper);
        
        var result = await articleService.GetLatestNews(default);

        result.Count.Should().Be(5);
    }
}