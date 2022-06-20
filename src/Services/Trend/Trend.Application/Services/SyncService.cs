﻿using AutoMapper;
using Dtos.Common.Shared;
using Dtos.Common.v1.Trend;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Interfaces;
using Trend.Application.Models.Service.Google;
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

        public SyncService(ILogger<SyncService> logger, 
            IMapper mapper,
            ISearchWordRepository syncSettingRepo,
            IGoogleSyncService googleSync,
            ISyncStatusRepository syncStatusRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _syncSettingRepo = syncSettingRepo;
            _googleSync = googleSync;
            _syncStatusRepo = syncStatusRepository;
        }

        public async Task<GoogleSyncResult> ExecuteGoogleSync()
        {
            _logger.LogTrace("Starting the sync process by fetching db instances");

            var searchWords = await _syncSettingRepo.GetAll();

            if(searchWords.Count == 0)
            {
                _logger.LogInformation("Array of searchwrods are empty. Sync process is stopped");
                throw new TrendAppCoreException("Array of searchwrods are empty. Sync process is stopped");
            }

            Dictionary<ContextType, List<string>> googleSyncRequest = searchWords
                .GroupBy(i => i.Type)
                .ToDictionary(i => i.Key, y => y.Select(i => i.Word).ToList());

            var syncResult = await _googleSync.Sync(googleSyncRequest);

            return syncResult;
        }

        public async Task<List<SyncStatusDto>> GetSyncStatuses()
        {
            _logger.LogTrace("Fetching sync statuses from DB");

            var entities = await _syncStatusRepo.GetAll();

            if(entities.Count == 0)
            {
                _logger.LogInformation("No sync statuses");
                return new List<SyncStatusDto>();
            }

            _logger.LogTrace("Mapping entities to dto");

            var dtos = _mapper.Map<List<SyncStatusDto>>(entities);

            return dtos;
        }

        public async Task<List<SyncStatusWordDto>> GetSyncStatusSearchWords(string syncStatusId)
        {
            _logger.LogTrace("Fetching sync status");

            var syncStatus = await _syncStatusRepo.FindById(syncStatusId);

            if(syncStatus is null)
            {
                _logger.LogInformation("Sync status with ID {0} not found", syncStatusId);
                throw new TrendNotFoundException();
            }

            _logger.LogTrace("Fetching sync status words");

            var syncWords = await _syncStatusRepo.GetSyncStatusWords(syncStatusId);

            if(syncWords.Count == 0)
            {
                _logger.LogTrace("Zero items find in database");
                return new List<SyncStatusWordDto>();
            }

            _logger.LogTrace("Mapping entities to dtos");

            var response = _mapper.Map<List<SyncStatusWordDto>>(syncWords);

            return response;
        }
    }
}
