using AutoMapper;
using Dtos.Common.Shared;
using Dtos.Common.v1.Trend.Sync;
using Events.Common.Trend;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.OutputCaching;
using Trend.Application.Interfaces;
using Trend.Application.Models.Service.Intern.Google;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Exceptions;
using Trend.Domain.Interfaces;

namespace Trend.Application.Services
{
    public class SyncService : ISyncService
    {
        private readonly ILogger<SyncService> _logger;
        private readonly IMapper _mapper;
        private readonly ISearchWordRepository _syncSettingRepo;
        private readonly ISyncStatusRepository _syncStatusRepo;
        private readonly IEnumerable<ISearchEngine> _searchEngines;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IOutputCacheStore _cacheStore;

        public SyncService(ILogger<SyncService> logger, 
            IMapper mapper,
            ISearchWordRepository syncSettingRepo,
            IEnumerable<ISearchEngine> searchEngines,
            ISyncStatusRepository syncStatusRepository,
            IPublishEndpoint publishEndpoint,
            IOutputCacheStore cacheStore)
        {
            _logger = logger;
            _mapper = mapper;
            _syncSettingRepo = syncSettingRepo;
            _searchEngines = searchEngines;
            _syncStatusRepo = syncStatusRepository;
            _publishEndpoint = publishEndpoint;
            _cacheStore = cacheStore;
        }

        private async Task<bool> FireSearchEngines(Dictionary<ContextType, List<string>> searchWords, CancellationToken token)
        {
            var successNum = 0;
            foreach (var searchEngine in _searchEngines)
            {
                try
                {
                    await searchEngine.Sync(searchWords, token);
                    successNum++;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                }
            }

            return successNum > 0;
        }
        
        public async Task ExecuteSync(CancellationToken token)
        {
            var searchWords = await _syncSettingRepo.GetAll(token);

            if(searchWords.Count == 0)
            {
                _logger.LogInformation("Array of search words is empty. Sync process is stopped");
                throw new TrendAppCoreException("Array of search words is empty. Sync process is stopped");
            }

            var syncRequest = searchWords
                .GroupBy(i => i.Type)
                .ToDictionary(i => i.Key, y => y.Select(i => i.Word).ToList());

            var anyUpdates = await FireSearchEngines(syncRequest, token);
            if (anyUpdates)
            {
                await _cacheStore.EvictByTagAsync("Sync", default);
                await _cacheStore.EvictByTagAsync("News", default);
                await _publishEndpoint.Publish(new NewNewsFetched { }, token);
            }
        }

        public async Task<SyncStatusDto> GetSync(string id, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogInformation("Sync id not provided");
                return null;
            }

            var entity = await _syncStatusRepo.FindById(id, token);
            if (entity is not null) return _mapper.Map<SyncStatusDto>(entity);
            _logger.LogInformation("Sync with provided ID {0} not found", id);
            return null;
        }

        public async Task<List<SyncStatusDto>> GetSyncStatuses(CancellationToken token)
        {
            var entities = await _syncStatusRepo.GetAll(token);

            if(entities.Count == 0)
            {
                _logger.LogInformation("No sync statuses");
                return new List<SyncStatusDto>();
            }

            var dtos = _mapper.Map<List<SyncStatusDto>>(entities);
            return dtos;
        }

        public async Task<PageResponseDto<SyncStatusDto>> GetSyncStatusesPage(PageRequestDto request, CancellationToken token)
        {
            var entitiesPage = await _syncStatusRepo.FilterBy(request.Page, request.Take, null, token);
            var dtoPage = _mapper.Map<PageResponseDto<SyncStatusDto>>(entitiesPage);
            return dtoPage;
        }

        public async Task<List<SyncStatusWordDto>> GetSyncStatusSearchWords(string syncStatusId, CancellationToken token)
        {
            var syncStatus = await _syncStatusRepo.FindById(syncStatusId, token);
            if(syncStatus is null)
            {
                _logger.LogInformation("Sync status with ID {0} not found", syncStatusId);
                throw new TrendNotFoundException();
            }

            var syncWords = await _syncStatusRepo.GetSyncStatusWords(syncStatusId, token);
            if(syncWords.Count == 0)
            {
                _logger.LogTrace("Zero items find in database");
                return new List<SyncStatusWordDto>();
            }

            var response = _mapper.Map<List<SyncStatusWordDto>>(syncWords);
            return response;
        }
    }
}
