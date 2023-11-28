using AutoFixture;
using AutoMapper;
using Dtos.Common.v1.Trend.SearchWord;
using FluentAssertions;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Trend.Application.Services;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Exceptions;
using Trend.Domain.Interfaces;

namespace Trend.UnitTests.Application;

public class SearchWordServiceTests
{
    private readonly ILogger<SearchWordService> _logger  = Substitute.For<ILogger<SearchWordService>>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly Fixture _fixture = new();
    private readonly ISearchWordRepository _wordRepository = Substitute.For<ISearchWordRepository>();
    private readonly IOutputCacheStore _cacheStore = Substitute.For<IOutputCacheStore>();

    [Fact]
    public async Task AddNewSyncSetting_ShouldReturnCreatedInstance_WhenUnExistingInstanceProvided()
    {
        var request = new SearchWordCreateDto
        {
            SearchWord = "BTC",
            ContextType = (int)ContextType.Crypto,
            SearchEngine = (int)SearchEngine.Google
        };
        
        _wordRepository.IsDuplicate(Arg.Any<string>(), Arg.Any<SearchEngine>(), Arg.Any<CancellationToken>())
            .Returns(false);
        _mapper.Map<SearchWordDto>(Arg.Any<SearchWord>())
            .Returns(t => new SearchWordDto
            {
                SearchWord = "BTC",
                SearchEngineId = (int)SearchEngine.Google,
            });
        
        var service = new SearchWordService(_logger, _mapper, _wordRepository, _cacheStore);
        
        var response = await service.AddNewSyncSetting(request, default);

        response.SearchWord.Should().Be(request.SearchWord);
        response.SearchEngineId.Should().Be(request.SearchEngine);
    }
    
    [Fact]
    public async Task AddNewSyncSetting_ShouldThrowException_WhenExistingInstanceProvided()
    {
        var request = new SearchWordCreateDto
        {
            SearchWord = "BTC",
            ContextType = (int)ContextType.Crypto,
            SearchEngine = (int)SearchEngine.Google
        };
        
        _wordRepository.IsDuplicate(Arg.Any<string>(), Arg.Any<SearchEngine>(), Arg.Any<CancellationToken>())
            .Returns(true);
        
        var service = new SearchWordService(_logger, _mapper, _wordRepository, _cacheStore);

        await Assert.ThrowsAsync<TrendAppCoreException>(() => service.AddNewSyncSetting(request, default));
    }
    
    [Fact]
    public async Task GetAvailableContextTypes_ShouldReturnList_WhenInvoked()
    {
        var service = new SearchWordService(_logger, _mapper, _wordRepository, _cacheStore);

        var response = await service.GetAvailableContextTypes(default);

        response.Count.Should().BeGreaterThan(0);
        response.Should().Contain(x => x.Value == "Crypto");
    }
    
    [Fact]
    public async Task GetAvailableSearchEngines_ShouldReturnList_WhenInvoked()
    {
        var service = new SearchWordService(_logger, _mapper, _wordRepository, _cacheStore);

        var response = await service.GetAvailableSearchEngines(default);

        response.Count.Should().BeGreaterThan(0);
        response.Should().Contain(x => x.Value == "Google");
    }
    
    [Fact]
    public async Task GetSyncSettingsWords_ShouldReturnList_WhenInvoked()
    {
        _wordRepository.IsDuplicate(Arg.Any<string>(), Arg.Any<SearchEngine>(), Arg.Any<CancellationToken>())
            .Returns(true);
        
        _mapper.Map<List<SearchWordDto>>(Arg.Any<List<SearchWord>>())
            .Returns(t => _fixture.CreateMany<SearchWordDto>(5).ToList());
        
        var service = new SearchWordService(_logger, _mapper, _wordRepository, _cacheStore);

        var response = await service.GetSyncSettingsWords(default);

        response.Should().BeOfType<List<SearchWordDto>>();
    }
    
    [Fact]
    public async Task RemoveSyncSetting_ShouldThrowTrendAppCoreException_WhenIdNullOrEmpty()
    {
        string? itemId = default;
        
        var service = new SearchWordService(_logger, _mapper, _wordRepository, _cacheStore);

        await Assert.ThrowsAsync<TrendAppCoreException>(() => service.RemoveSyncSetting(itemId, default));
    }
    
    [Fact]
    public async Task RemoveSyncSetting_ShouldThrowTrendNotFoundException_WhenIdDontExists()
    {
        string itemId = _fixture.Create<string>();
        
        _wordRepository.FindById(itemId, Arg.Any<CancellationToken>()).Returns(Task.FromResult<SearchWord>(null!));
        
        var service = new SearchWordService(_logger, _mapper, _wordRepository, _cacheStore);

        await Assert.ThrowsAsync<TrendNotFoundException>(() => service.RemoveSyncSetting(itemId, default));
    }
    
    [Fact]
    public async Task RemoveSyncSetting_ShouldNotThrowException_WhenExistingIdProvided()
    {
        string itemId = _fixture.Create<string>();
        
        _wordRepository.FindById(itemId, Arg.Any<CancellationToken>()).Returns(Task.FromResult(_fixture.Create<SearchWord>()));
        
        var service = new SearchWordService(_logger, _mapper, _wordRepository, _cacheStore);

        await service.RemoveSyncSetting(itemId, default);
    }
}