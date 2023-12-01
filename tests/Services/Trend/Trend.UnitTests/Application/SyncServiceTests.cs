using AutoFixture;
using AutoMapper;
using Events.Common.Trend;
using Google.Protobuf.WellKnownTypes;
using MassTransit;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Time.Abstract.Contracts;
using Trend.Application.Interfaces;
using Trend.Application.Services;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Exceptions;
using Trend.Domain.Interfaces;

namespace Trend.UnitTests.Application;

public class SyncServiceTests
{
    private readonly ILogger<SyncService> _logger;
    private readonly IMapper _mapper;
    private readonly ISearchWordRepository _syncSettingRepo;
    private readonly IArticleRepository _articleRepo;
    private readonly IEnumerable<ISearchEngine> _searchEngines;
    private readonly ISyncStatusRepository _syncStatusRepo;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IOutputCacheStore _cacheStore;
    private readonly IDateTimeProvider _timeProvider;
    private readonly ITransaction _transaction;
    private readonly Fixture _fixture = new();
    
    public SyncServiceTests()
    {
        _logger = Substitute.For<ILogger<SyncService>>();
        _mapper = Substitute.For<IMapper>();
        _syncSettingRepo = Substitute.For<ISearchWordRepository>();
        _syncStatusRepo = Substitute.For<ISyncStatusRepository>();
        _articleRepo = Substitute.For<IArticleRepository>();
        _publishEndpoint = Substitute.For<IPublishEndpoint>();
        _cacheStore = Substitute.For<IOutputCacheStore>();
        _timeProvider = Substitute.For<IDateTimeProvider>();
        _transaction = Substitute.For<ITransaction>();
        
        var engine = Substitute.For<ISearchEngine>();
        _searchEngines = new[] { engine };
    }

    [Fact]
    public async Task ExecuteSync_ShouldNotThrowException_WhenThereAreAvailableSearchWords()
    {
        var searchWords = _fixture.CreateMany<SearchWord>(5).ToList();
        _syncSettingRepo.GetAll(Arg.Any<CancellationToken>()).Returns(Task.FromResult(searchWords));
        _articleRepo.GetActiveArticles(Arg.Any<CancellationToken>()).Returns(_fixture.CreateMany<Article>().ToList());
        
        foreach (var engineMock in _searchEngines)
        {
            engineMock.Sync(Arg.Any<Dictionary<ContextType, List<string>>>(), Arg.Any<CancellationToken>())
                .Returns((_fixture.Create<SyncStatus>(), _fixture.CreateMany<Article>(5).ToList()));
        }
        
        var syncService = new SyncService(_logger, _mapper, _syncSettingRepo, _searchEngines, 
            _syncStatusRepo, _publishEndpoint, _cacheStore, _timeProvider, _articleRepo, _transaction);
        
        await syncService.ExecuteSync(CancellationToken.None);
    }
    
    
    [Fact]
    public async Task ExecuteSync_ShouldThrowTrendAppCoreException_WhenThereAreNoAvailableSearchWords()
    {
        _syncSettingRepo.GetAll(Arg.Any<CancellationToken>()).Returns(Task.FromResult(new List<SearchWord>()));

        var syncService = new SyncService(_logger, _mapper, _syncSettingRepo, _searchEngines, _syncStatusRepo, 
            _publishEndpoint, _cacheStore, _timeProvider, _articleRepo, _transaction);

        await Assert.ThrowsAsync<TrendAppCoreException>(() => syncService.ExecuteSync(CancellationToken.None));
    }
}