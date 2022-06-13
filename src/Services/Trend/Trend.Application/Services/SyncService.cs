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

        public SyncService(ILogger<SyncService> logger, IMapper mapper, ISyncSettingRepository syncSettingRepo)
        {
            _logger = logger;
            _mapper = mapper;
            _syncSettingRepo = syncSettingRepo;
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
            throw new NotImplementedException();
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
    }
}
