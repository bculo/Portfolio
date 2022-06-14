using AutoMapper;
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
        private readonly ISyncSettingRepository _syncSettingRepo;
        private readonly ISyncStatusRepository _syncStatusRepo;
        private readonly IGoogleSyncService _googleSync;

        public SyncService(ILogger<SyncService> logger, 
            IMapper mapper,
            ISyncSettingRepository syncSettingRepo,
            IGoogleSyncService googleSync,
            ISyncStatusRepository syncStatusRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _syncSettingRepo = syncSettingRepo;
            _googleSync = googleSync;
            _syncStatusRepo = syncStatusRepository;
        }

        public async Task<SyncSettingDto> AddNewSyncSetting(SyncSettingCreateDto instance)
        {
            _logger.LogTrace("Checking if duplicate");

            var isDuplicate = await _syncSettingRepo.IsDuplicate(instance.SearchWord, (SearchEngine)instance.SearchEngine);

            if (isDuplicate)
            {
                _logger.LogInformation("Sync setting with given search word and engine needs to be unqiue");
                throw new TrendAppCoreException("Sync setting with given search word and engine needs to be unqiue");
            }

            var entity = _mapper.Map<SyncSetting>(instance);

            _logger.LogTrace("Adding new sync setting to DB");

            await _syncSettingRepo.Add(entity);

            var response = _mapper.Map<SyncSettingDto>(entity);

            return response;
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
                .ToDictionary(i => i.Key, y => y.Select(i => i.SearchWord).ToList());

            var syncResult = await _googleSync.Sync(googleSyncRequest);

            return syncResult;
        }

        public async Task<List<SyncSettingDto>> GetSyncSettingsWords()
        {
            _logger.LogTrace("Fetching sync settings words");

            var entities = await _syncSettingRepo.GetAll();

            _logger.LogTrace("Mapping entities to dtos");

            var dtos = _mapper.Map<List<SyncSettingDto>>(entities);

            return dtos;
        }

        public async Task<List<KeyValueElementDto>> GetAvailableSearchEngines()
        {
            _logger.LogTrace("Mapping enum to dto");

            return Enum.GetValues<SearchEngine>().Select(i => new KeyValueElementDto
            {
                Key = (int)i,
                Value = i.ToString()
            }).ToList();
        }

        public async Task RemoveSyncSetting(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogInformation("ID {0} is null or empty", id);
                throw new TrendAppCoreException("Given id is invalid");
            }

            _logger.LogTrace("Fetching item with {0}", id);

            var entity = await _syncSettingRepo.FindById(id);

            if(entity is null)
            {
                _logger.LogInformation("Item with given id {0} not found", id);
                throw new TrendNotFoundException();
            }

            _logger.LogTrace("Removing item");

            await _syncSettingRepo.Delete(entity.Id);
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

        public async Task<List<KeyValueElementDto>> GetAvaiableContextTypes()
        {
            _logger.LogTrace("Mapping enum to dto");

            return Enum.GetValues<ContextType>().Select(i => new KeyValueElementDto
            {
                Key = (int)i,
                Value = i.ToString()
            }).ToList();
        }
    }
}
