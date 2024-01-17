using AutoMapper;
using Dtos.Common;
using Events.Common.Trend;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.OutputCaching;
using Time.Abstract.Contracts;
using Trend.Application.Configurations.Constants;
using Trend.Application.Consumers;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models.Dtos;
using Trend.Application.Interfaces.Models.Services;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Exceptions;

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
        
        public async Task ExecuteSync(CancellationToken token)
        {
            using var span = Telemetry.Trend.StartActivity(Telemetry.SYNC_SRV);
            
            var searchWords = await _syncSettingRepo.GetActiveItems(token);

            if(searchWords.Count == 0)
            {
                span?.SetTag(Telemetry.SYNC_SRV_NUM_TAG, 0);
                throw new TrendAppCoreException("Array of search words is empty. Sync process is stopped");
            }
            
            var engineRequest = searchWords
                .GroupBy(i => i.Type)
                .ToDictionary(i => i.Key, y => y.Select(i => new SearchEngineWord
                {
                    SearchWord = i.Word,
                    SearchWordId = i.Id
                }).ToList());

            span?.SetTag(Telemetry.SYNC_SRV_NUM_TAG, engineRequest.Keys.Count);
            
            var oldActiveArticles = await _articleRepo.GetActiveItems(token);

            var (syncs, articles) = await FireSearchEngines(engineRequest, token);

            await CheckForTotalFailure(syncs, token);
            
            await _session.StartTransaction();

            await PersistSyncStatuses(syncs, token);
            await PersistNewArticles(articles, token);
            await DeactivateOldArticles(oldActiveArticles, token);
            
            await _session.CommitTransaction();
            
            await InvalidateCache(token);
            
            await _publishEndpoint.Publish<SyncExecuted>(new(), token);
        }

        private async Task CheckForTotalFailure(List<SyncStatus> syncs, CancellationToken token)
        {
            var isTotalFailure = syncs.All(x => x.SucceddedRequests == 0);
            if (isTotalFailure)
            {
                await _publishEndpoint.Publish<TotalSyncFailure>(new(), token);
            }
        }

        private async Task PersistSyncStatuses(List<SyncStatus> syncs, CancellationToken token)
        {
            await _syncStatusRepo.Add(syncs, token);
        }

        private async Task PersistNewArticles(List<Article> articles, CancellationToken token)
        {
            await _articleRepo.Add(articles, token);
        }

        private async Task DeactivateOldArticles(List<Article> oldActiveArticles, CancellationToken token)
        {
            var oldActiveIds = oldActiveArticles.Select(i => i.Id).ToList();
            await _articleRepo.DeactivateItems(oldActiveIds, token);
        }

        private async Task InvalidateCache(CancellationToken token)
        {
            await _cacheStore.EvictByTagAsync(CacheTags.SYNC, default);
            await _cacheStore.EvictByTagAsync(CacheTags.NEWS, default);
        }
        
        private async Task<(List<SyncStatus> syncIterations, List<Article> articles)> FireSearchEngines(
            Dictionary<ContextType, List<SearchEngineWord>> searchWords, 
            CancellationToken token)
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
                    await RaiseFailureEvent(searchEngine.EngineName, token);
                }
            }

            return (syncIterations, articles);
        }

        private async Task RaiseFailureEvent(string engineName, CancellationToken token)
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

        public async Task<SyncStatusResDto> GetSync(string id, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new TrendNotFoundException($"Sync with {id} not found");
            }

            var entity = await _syncStatusRepo.FindById(id, token);
            if (entity is not null) return _mapper.Map<SyncStatusResDto>(entity);
            _logger.LogInformation("Sync with provided ID {0} not found", id);
            throw new TrendNotFoundException($"Sync with {id} not found");
        }

        public async Task<List<SyncStatusResDto>> GetSyncStatuses(CancellationToken token)
        {
            var entities = await _syncStatusRepo.GetAll(token);

            if(entities.Count == 0)
            {
                return new List<SyncStatusResDto>();
            }

            var instances = _mapper.Map<List<SyncStatusResDto>>(entities);
            return instances;
        }

        public async Task<PageResponseDto<SyncStatusResDto>> GetSyncStatusesPage(PageRequestDto request, CancellationToken token)
        {
            var entitiesPage = await _syncStatusRepo.FilterBy(request.Page, request.Take, null, token);
            var dtoPage = _mapper.Map<PageResponseDto<SyncStatusResDto>>(entitiesPage);
            return dtoPage;
        }

        public async Task<List<SyncStatusWordResDto>> GetSyncStatusSearchWords(string syncStatusId, CancellationToken token)
        {
            var syncStatus = await _syncStatusRepo.FindById(syncStatusId, token);
            if(syncStatus is null)
            {
                throw new TrendNotFoundException($"Sync status with ID {syncStatusId} not found");
            }

            var syncWords = await _syncStatusRepo.GetSyncStatusWords(syncStatusId, token);
            if(syncWords.Count == 0)
            {
                return new List<SyncStatusWordResDto>();
            }

            var response = _mapper.Map<List<SyncStatusWordResDto>>(syncWords);
            return response;
        }
    }
}
