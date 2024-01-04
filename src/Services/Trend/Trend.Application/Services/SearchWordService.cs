using AutoMapper;
using Dtos.Common;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Trend.Application.Configurations.Constants;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models.Dtos;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Exceptions;

namespace Trend.Application.Services
{
    public class SearchWordService : ISearchWordService
    {
        private readonly ILogger<SearchWordService> _logger;
        private readonly IMapper _mapper;
        private readonly ISearchWordRepository _wordRepository;
        private readonly IOutputCacheStore _cacheStore;

        public SearchWordService(ILogger<SearchWordService> logger,
            IMapper mapper,
            ISearchWordRepository wordRepository,
            IOutputCacheStore cacheStore)
        {
            _logger = logger;
            _mapper = mapper;
            _wordRepository = wordRepository;
            _cacheStore = cacheStore;
        }

        public async Task<SearchWordResDto> AddNewSyncSetting(SearchWordCreateReqDto instance, CancellationToken token)
        {
            var isDuplicate = await _wordRepository.IsDuplicate(instance.SearchWord, (SearchEngine)instance.SearchEngine, token);

            if (isDuplicate)
            {
                throw new TrendAppCoreException("Sync setting with given search word and engine needs to be unique");
            }

            var entity = _mapper.Map<SearchWord>(instance);
            await _wordRepository.Add(entity, token);
            await _cacheStore.EvictByTagAsync(CacheTags.SEARCH_WORD, default);
            var response = _mapper.Map<SearchWordResDto>(entity);
            return response;
        }

        public Task<List<KeyValueElementDto>> GetAvailableContextTypes(CancellationToken token)
        {
            return Task.FromResult(Enum.GetValues<ContextType>().Select(i => new KeyValueElementDto
            {
                Key = (int)i,
                Value = i.ToString()
            }).ToList());
        }

        public Task<List<KeyValueElementDto>> GetAvailableSearchEngines(CancellationToken token)
        {
            return Task.FromResult(Enum.GetValues<Domain.Enums.SearchEngine>().Select(i => new KeyValueElementDto
            {
                Key = (int)i,
                Value = i.ToString()
            }).ToList());
        }

        public async Task<List<SearchWordResDto>> GetSyncSettingsWords(CancellationToken token)
        {
            var entities = await _wordRepository.GetAll(token);
            var dtos = _mapper.Map<List<SearchWordResDto>>(entities);
            return dtos;
        }

        public async Task RemoveSyncSetting(string id, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogInformation("ID {0} is null or empty", id);
                throw new TrendAppCoreException("Given id is invalid");
            }

            var entity = await _wordRepository.FindById(id, token);

            if (entity is null)
            {
                _logger.LogInformation("Item with given id {0} not found", id);
                throw new TrendNotFoundException();
            }

            await _wordRepository.Delete(entity.Id, token);
            await _cacheStore.EvictByTagAsync(CacheTags.SEARCH_WORD, default);
        }
    }
}
