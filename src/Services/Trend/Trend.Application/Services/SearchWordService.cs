using AutoMapper;
using Dtos.Common;
using MassTransit.Topology;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trend.Application.Configurations.Constants;
using Trend.Application.Configurations.Options;
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
        private readonly IImageService _imageService;
        private readonly IBlobStorage _blobStorage;
        private readonly BlobStorageOptions _storageOptions;

        public SearchWordService(ILogger<SearchWordService> logger,
            IMapper mapper,
            ISearchWordRepository wordRepository,
            IOutputCacheStore cacheStore, 
            IImageService imageService, 
            IBlobStorage blobStorage,
            IOptions<BlobStorageOptions> storageOptions)
        {
            _logger = logger;
            _mapper = mapper;
            _wordRepository = wordRepository;
            _cacheStore = cacheStore;
            _imageService = imageService;
            _blobStorage = blobStorage;
            _storageOptions = storageOptions.Value;
        }

        public async Task<SearchWordResDto> AddNewSearchWord(SearchWordAddReqDto instance, CancellationToken token)
        {
            var isDuplicate = await _wordRepository.IsDuplicate(instance.SearchWord, (SearchEngine)instance.SearchEngine, token);

            if (isDuplicate)
            {
                throw new TrendAppCoreException("Sync setting with given search word and engine needs to be unique");
            }

            var entity = _mapper.Map<SearchWord>(instance);
            await _wordRepository.Add(entity, token);
            
            await _cacheStore.EvictByTagAsync(CacheTags.SEARCH_WORD, default);
            
            return _mapper.Map<SearchWordResDto>(entity);
        }

        public async Task AttachImageToSearchWord(SearchWordAttachImageReqDto req, CancellationToken token)
        {
            var instance = await _wordRepository.FindById(req.SearchWordId, token);
            
            if (instance is null || !instance.IsActive)
            {
                throw new TrendNotFoundException();
            }
            
            await using var stream = await _imageService.ResizeImage(req.Content, 720, 480);
            
            var uri = await _blobStorage.UploadBlob(_storageOptions.TrendContainerName,
                instance.Word, 
                stream, 
                req.ContentType);

            instance.ImageUrl = uri.ToString();

            await _wordRepository.Update(instance, token);
            
            await _cacheStore.EvictByTagAsync(CacheTags.SEARCH_WORD, default);
        }

        public async Task ActivateSearchWord(string id, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogInformation("ID {0} is null or empty", id);
                throw new TrendAppCoreException("Given id is invalid");
            }

            var entity = await _wordRepository.FindById(id, token);

            if (entity is null || entity.IsActive)
            {
                _logger.LogInformation("Item with given id {0} not found", id);
                throw new TrendNotFoundException();
            }

            await _wordRepository.ActivateItems(new List<string> { entity.Id }, token);
            await _cacheStore.EvictByTagAsync(CacheTags.SEARCH_WORD, default);
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
            return Task.FromResult(Enum.GetValues<SearchEngine>().Select(i => new KeyValueElementDto
            {
                Key = (int)i,
                Value = i.ToString()
            }).ToList());
        }

        public async Task<List<SearchWordResDto>> GetSearchWords(CancellationToken token)
        {
            var entities = await _wordRepository.GetActiveItems(token);
            var instances = _mapper.Map<List<SearchWordResDto>>(entities);
            return instances;
        }

        public async Task DeactivateSearchWord(string id, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogInformation("ID {0} is null or empty", id);
                throw new TrendAppCoreException("Given id is invalid");
            }

            var entity = await _wordRepository.FindById(id, token);

            if (entity is null || !entity.IsActive)
            {
                _logger.LogInformation("Item with given id {0} not found", id);
                throw new TrendNotFoundException();
            }

            await _wordRepository.DeactivateItems(new List<string> { entity.Id }, token);
            await _cacheStore.EvictByTagAsync(CacheTags.SEARCH_WORD, default);
        }
    }
}
