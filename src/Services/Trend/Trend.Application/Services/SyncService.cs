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
        private readonly IGoogleSyncService _googleSync;
        private readonly IPublishEndpoint _publishEndpoint;

        public SyncService(ILogger<SyncService> logger, 
            IMapper mapper,
            ISearchWordRepository syncSettingRepo,
            IGoogleSyncService googleSync,
            ISyncStatusRepository syncStatusRepository,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _mapper = mapper;
            _syncSettingRepo = syncSettingRepo;
            _googleSync = googleSync;
            _syncStatusRepo = syncStatusRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<SyncResultDto> ExecuteSync()
        {
            var searchWords = await _syncSettingRepo.GetAll();

            if(searchWords.Count == 0)
            {
                _logger.LogInformation("Array of search words is empty. Sync process is stopped");
                throw new TrendAppCoreException("Array of search words is empty. Sync process is stopped");
            }

            var googleSyncRequest = searchWords
                .GroupBy(i => i.Type)
                .ToDictionary(i => i.Key, y => y.Select(i => i.Word).ToList());

            var syncResult = await _googleSync.Sync(googleSyncRequest);

            var dto = _mapper.Map<SyncResultDto>(syncResult);

            await _publishEndpoint.Publish(new NewNewsFetched { });

            return dto;
        }

        public async Task<SyncStatusDto> GetSync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogInformation("Sync id not provided");
                return null;
            }

            var entity = await _syncStatusRepo.FindById(id);
            if (entity is not null) return _mapper.Map<SyncStatusDto>(entity);
            _logger.LogInformation("Sync with provided ID {0} not found", id);
            return null;
        }

        public async Task<List<SyncStatusDto>> GetSyncStatuses()
        {
            var entities = await _syncStatusRepo.GetAll();

            if(entities.Count == 0)
            {
                _logger.LogInformation("No sync statuses");
                return new List<SyncStatusDto>();
            }

            var dtos = _mapper.Map<List<SyncStatusDto>>(entities);
            return dtos;
        }

        public async Task<PageResponseDto<SyncStatusDto>> GetSyncStatusesPage(PageRequestDto request)
        {
            var entitiesPage = await _syncStatusRepo.FilterBy(request.Page, request.Take);
            var dtoPage = _mapper.Map<PageResponseDto<SyncStatusDto>>(entitiesPage);
            return dtoPage;
        }

        public async Task<List<SyncStatusWordDto>> GetSyncStatusSearchWords(string syncStatusId)
        {
            var syncStatus = await _syncStatusRepo.FindById(syncStatusId);
            if(syncStatus is null)
            {
                _logger.LogInformation("Sync status with ID {0} not found", syncStatusId);
                throw new TrendNotFoundException();
            }

            var syncWords = await _syncStatusRepo.GetSyncStatusWords(syncStatusId);
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
