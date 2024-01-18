using AutoMapper;
using Dtos.Common;
using Events.Common.Trend;
using LanguageExt;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.OutputCaching;
using OpenTelemetry.Trace;
using Time.Abstract.Contracts;
using Trend.Application.Configurations.Constants;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models.Dtos;
using Trend.Application.Interfaces.Models.Services;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Errors;
using Trend.Domain.Exceptions;
using Activity = System.Diagnostics.Activity;

namespace Trend.Application.Services
{
    public class SyncService : ISyncService
    {
        private readonly ILogger<SyncService> _logger;
        private readonly IMapper _mapper;
        private readonly ISearchWordRepository _syncSettingRepo;
        private readonly ISyncStatusRepository _syncStatusRepo;
        private readonly IArticleRepository _articleRepo;
        private readonly IEnumerable<ISearchEngine> _searchEngines;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IOutputCacheStore _cacheStore;
        private readonly IDateTimeProvider _provider;
        private readonly ITransaction _session;

        public SyncService(ILogger<SyncService> logger, 
            IMapper mapper,
            ISearchWordRepository syncSettingRepo,
            IEnumerable<ISearchEngine> searchEngines,
            ISyncStatusRepository syncStatusRepository,
            IPublishEndpoint publishEndpoint,
            IOutputCacheStore cacheStore, 
            IDateTimeProvider provider, 
            IArticleRepository articleRepo, 
            ITransaction session)
        {
            _logger = logger;
            _mapper = mapper;
            _syncSettingRepo = syncSettingRepo;
            _searchEngines = searchEngines;
            _syncStatusRepo = syncStatusRepository;
            _publishEndpoint = publishEndpoint;
            _cacheStore = cacheStore;
            _provider = provider;
            _articleRepo = articleRepo;
            _session = session;
        }
        
        public async Task<Either<CoreError, Unit>> ExecuteSync(CancellationToken token = default)
        {
            using var span = Telemetry.Trend.StartActivity(Telemetry.SYNC_SRV);
            span?.SetTag(Telemetry.SYNC_SRV_NUM_TAG, _searchEngines.Count());
            
            var searchWords = await _syncSettingRepo.GetActiveItems(token);
            if(searchWords.Count == 0)
            {
                _logger.LogInformation("Array of search words is empty. Sync process is stopped");
                return SyncErrors.NoSearchWords;
            }

            var searchEngineRequest = MapToSearchEngineRequest(searchWords);
            var (syncs, articles) = await FireSearchEngines(searchEngineRequest, token);
            
            try
            {
                await _session.StartTransaction(token);

                var oldActiveArticles = await _articleRepo.GetActiveItems(token);
                await DeactivateOldArticles(oldActiveArticles, token);
                await PersistSyncStatuses(syncs, token);
                await PersistNewArticles(articles, token);

                await PublishSyncExecutedEvent(token);
                
                await _session.CommitTransaction(token);
            }
            catch
            {
                await _session.AbortTransaction(token);
                throw;
            }
            
            await InvalidateCache(token);
            return Unit.Default;
        }

        private Dictionary<ContextType, List<SearchEngineWord>> MapToSearchEngineRequest(List<SearchWord> searchWords)
        {
            return searchWords
                .GroupBy(i => i.Type)
                .ToDictionary(
                    i => i.Key, 
                    y => y.Select(i => new SearchEngineWord
                    {
                        SearchWord = i.Word,
                        SearchWordId = i.Id
                    }).ToList()
                );
        }

        private async Task PublishSyncExecutedEvent(CancellationToken token = default)
        {
            await _publishEndpoint.Publish<SyncExecuted>(new(), token);
        }

        private async Task CheckForTotalFailure(List<SyncStatus> syncs, CancellationToken token = default)
        {
            var isTotalFailure = syncs.All(x => x.SucceddedRequests == 0);
            if (isTotalFailure)
            {
                await _publishEndpoint.Publish<TotalSyncFailure>(new(), token);
            }
        }

        private async Task PersistSyncStatuses(List<SyncStatus> syncs, CancellationToken token = default)
        {
            await _syncStatusRepo.Add(syncs, token);
        }

        private async Task PersistNewArticles(List<Article> articles, CancellationToken token = default)
        {
            await _articleRepo.Add(articles, token);
        }

        private async Task DeactivateOldArticles(List<Article> oldActiveArticles, CancellationToken token = default)
        {
            var oldActiveIds = oldActiveArticles.Select(i => i.Id).ToList();
            await _articleRepo.DeactivateItems(oldActiveIds, token);
        }

        private async Task InvalidateCache(CancellationToken token = default)
        {
            await _cacheStore.EvictByTagAsync(CacheTags.SYNC, default);
            await _cacheStore.EvictByTagAsync(CacheTags.NEWS, default);
        }
        
        private async Task<(List<SyncStatus> syncIterations, List<Article> articles)> FireSearchEngines(
            Dictionary<ContextType, List<SearchEngineWord>> searchWords, 
            CancellationToken token = default)
        {
            List<SyncStatus> syncIterations = new();
            List<Article> articles = new();
            foreach (var searchEngine in _searchEngines)
            {
                try
                {
                    var result = await searchEngine.Sync(searchWords, token);
                    
                    syncIterations.Add(result.SyncIteration);
                    articles.AddRange(result.Articles);

                    if (result.SyncIteration.SucceddedRequests == 0)
                    {
                        await RaiseFailureEvent(searchEngine.EngineName, token);   
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    Activity.Current?.RecordException(e);
                    await RaiseFailureEvent(searchEngine.EngineName, token);
                }
            }

            await CheckForTotalFailure(syncIterations, token);
            return (syncIterations, articles);
        }

        private async Task RaiseFailureEvent(string engineName, CancellationToken token = default)
        {
            await _publishEndpoint.Publish(new SearchEngineFailure
            {
                Message = $"Engine {engineName} failure. 0 items synced",
                Time = _provider.Now
            }, token);
        }

        public async Task<long> GetSyncCount(CancellationToken token)
        {
            return await _syncStatusRepo.Count(token);
        }

        public async Task<Either<CoreError, SyncStatusResDto>> GetSync(string id, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogInformation("Search word is null or empty");
                return SyncErrors.EmptyId;
            }

            var entity = await _syncStatusRepo.FindById(id, token);
            if (entity is null)
            {
                _logger.LogInformation("Sync with provided ID {SyncStatusId} not found", id);
                return SyncErrors.NotFound;
            } 
            
            return _mapper.Map<SyncStatusResDto>(entity);
        }

        public async Task<List<SyncStatusResDto>> GetSyncStatuses(CancellationToken token = default)
        {
            var entities = await _syncStatusRepo.GetAll(token);
            var instances = _mapper.Map<List<SyncStatusResDto>>(entities);
            return instances;
        }
        
        public async Task<Either<CoreError, List<SyncStatusWordResDto>>> GetSyncStatusSearchWords(string syncStatusId, CancellationToken token = default)
        {
            var syncStatus = await _syncStatusRepo.FindById(syncStatusId, token);
            if(syncStatus is null)
            {
                _logger.LogInformation("Sync {ArticleId} not found", syncStatusId);
                return SyncErrors.NotFound;
            }

            var syncWords = await _syncStatusRepo.GetSyncStatusWords(syncStatusId, token);
            var response = _mapper.Map<List<SyncStatusWordResDto>>(syncWords);
            return response;
        }
    }
}
