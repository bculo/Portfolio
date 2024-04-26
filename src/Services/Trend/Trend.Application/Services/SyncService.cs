using System.Diagnostics;
using AutoMapper;
using Events.Common.Trend;
using LanguageExt;
using MassTransit;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using Time.Abstract.Contracts;
using Trend.Application.Configurations.Constants;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Errors;

namespace Trend.Application.Services
{
    public class SyncService : ServiceBase, ISyncService
    {
        private readonly ILogger<SyncService> _logger;
        private readonly IMapper _mapper;
        private readonly ISearchWordRepository _syncSettingRepo;
        private readonly ISyncStatusRepository _syncStatusRepo;
        private readonly IArticleRepository _articleRepo;
        private readonly IEnumerable<ISearchEngine> _searchEngines;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IOutputCacheStore _cacheStore;
        private readonly IDateTimeProvider _timeProvider;
        private readonly ITransaction _session;

        public SyncService(ILogger<SyncService> logger, 
            IMapper mapper,
            ISearchWordRepository syncSettingRepo,
            IEnumerable<ISearchEngine> searchEngines,
            ISyncStatusRepository syncStatusRepository,
            IPublishEndpoint publishEndpoint,
            IOutputCacheStore cacheStore, 
            IDateTimeProvider timeProvider, 
            IArticleRepository articleRepo, 
            ITransaction session,
            IServiceProvider provider)
            : base(provider)
        {
            _logger = logger;
            _mapper = mapper;
            _syncSettingRepo = syncSettingRepo;
            _searchEngines = searchEngines;
            _syncStatusRepo = syncStatusRepository;
            _publishEndpoint = publishEndpoint;
            _cacheStore = cacheStore;
            _timeProvider = timeProvider;
            _articleRepo = articleRepo;
            _session = session;
        }
        
        public async Task<Either<CoreError, Unit>> ExecuteSync(CancellationToken token = default)
        {
            using var span = Telemetry.Trend.StartActivity(nameof(Telemetry.SYNC_SRV));
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

        private Dictionary<ContextType, List<SearchEngineReq>> MapToSearchEngineRequest(List<SearchWord> searchWords)
        {
            return searchWords
                .GroupBy(i => i.Type)
                .ToDictionary(
                    i => i.Key, 
                    y => y.Select(i => new SearchEngineReq
                    {
                        SearchWord = i.Word,
                        SearchWordId = i.Id
                    }).ToList()
                );
        }

        private async Task PublishSyncExecutedEvent(CancellationToken token = default)
        {
            await _publishEndpoint.Publish(new SyncExecuted
            {
                Time = _timeProvider.Utc
            }, token);
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
            Dictionary<ContextType, List<SearchEngineReq>> searchWords, 
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
                Time = _timeProvider.Now
            }, token);
        }

        public async Task<long> GetAllCount(CancellationToken token)
        {
            return await _syncStatusRepo.Count(token);
        }

        public async Task<Either<CoreError, SyncStatusResDto>> Get(GetSyncStatusReqDto req, CancellationToken token = default)
        {
            var validationResult = await Validate(req, token);
            if (!validationResult.IsValid)
            {
                _logger.LogInformation(LogTemplates.VALIDATION_ERROR_TEMP);
                return SyncErrors.ValidationError(validationResult.Errors);
            }

            var entity = await _syncStatusRepo.FindById(req.Id, token);
            if (entity is null)
            {
                _logger.LogInformation("Sync with provided ID {SyncStatusId} not found", req.Id);
                return SyncErrors.NotFound;
            } 
            
            return _mapper.Map<SyncStatusResDto>(entity);
        }

        public async Task<List<SyncStatusResDto>> GetAll(CancellationToken token = default)
        {
            var entities = await _syncStatusRepo.GetAll(token);
            return _mapper.Map<List<SyncStatusResDto>>(entities);
        }
        
        public async Task<Either<CoreError, List<SyncSearchWordResDto>>> GetSyncSearchWords(
            SyncSearchWordsReqDto req, 
            CancellationToken token = default)
        {
            var validationResult = await Validate(req, token);
            if (!validationResult.IsValid)
            {
                _logger.LogInformation(LogTemplates.VALIDATION_ERROR_TEMP);
                return SyncErrors.ValidationError(validationResult.Errors);
            }
            
            var syncStatus = await _syncStatusRepo.FindById(req.Id, token);
            if(syncStatus is null)
            {
                _logger.LogInformation("Sync {SyncId} not found", req.Id);
                return SyncErrors.NotFound;
            }

            var syncWords = await _syncStatusRepo.GetSyncStatusWords(req.Id, token);
            return _mapper.Map<List<SyncSearchWordResDto>>(syncWords);
        }
    }
}
