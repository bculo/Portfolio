using AutoMapper;
using Dtos.Common.Shared;
using Dtos.Common.v1.Trend.SearchWord;
using Microsoft.Extensions.Logging;
using Trend.Application.Interfaces;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Exceptions;
using Trend.Domain.Interfaces;

namespace Trend.Application.Services
{
    public class SearchWordService : ISearchWordService
    {
        private readonly ILogger<SearchWordService> _logger;
        private readonly IMapper _mapper;
        private readonly ISearchWordRepository _wordRepository;

        public SearchWordService(ILogger<SearchWordService> logger,
            IMapper mapper,
            ISearchWordRepository wordRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _wordRepository = wordRepository;
        }

        public async Task<SearchWordDto> AddNewSyncSetting(SearchWordCreateDto instance)
        {
            var isDuplicate = await _wordRepository.IsDuplicate(instance.SearchWord, (SearchEngine)instance.SearchEngine);

            if (isDuplicate)
            {
                throw new TrendAppCoreException("Sync setting with given search word and engine needs to be unique");
            }

            var entity = _mapper.Map<SearchWord>(instance);
            await _wordRepository.Add(entity);
            var response = _mapper.Map<SearchWordDto>(entity);
            return response;
        }

        public Task<List<KeyValueElementDto>> GetAvailableContextTypes()
        {
            return Task.FromResult(Enum.GetValues<ContextType>().Select(i => new KeyValueElementDto
            {
                Key = (int)i,
                Value = i.ToString()
            }).ToList());
        }

        public Task<List<KeyValueElementDto>> GetAvailableSearchEngines()
        {
            return Task.FromResult(Enum.GetValues<SearchEngine>().Select(i => new KeyValueElementDto
            {
                Key = (int)i,
                Value = i.ToString()
            }).ToList());
        }

        public async Task<List<SearchWordDto>> GetSyncSettingsWords()
        {
            var entities = await _wordRepository.GetAll();
            var dtos = _mapper.Map<List<SearchWordDto>>(entities);
            return dtos;
        }

        public async Task RemoveSyncSetting(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogInformation("ID {0} is null or empty", id);
                throw new TrendAppCoreException("Given id is invalid");
            }

            var entity = await _wordRepository.FindById(id);

            if (entity is null)
            {
                _logger.LogInformation("Item with given id {0} not found", id);
                throw new TrendNotFoundException();
            }

            await _wordRepository.Delete(entity.Id);
        }
    }
}
