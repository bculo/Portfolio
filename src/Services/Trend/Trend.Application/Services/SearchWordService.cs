using AutoMapper;
using Cache.Abstract.Contracts;
using Dtos.Common;
using MassTransit.Topology;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trend.Application.Configurations.Constants;
using Trend.Application.Configurations.Options;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models.Dtos;
using Trend.Application.Interfaces.Models.Repositories;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Exceptions;
using ZiggyCreatures.Caching.Fusion;

namespace Trend.Application.Services
{
    public class SearchWordService : ISearchWordService
    {
        private readonly ILogger<SearchWordService> _logger;
        private readonly IMapper _mapper;
        private readonly ISearchWordRepository _wordRepository;
        private readonly IOutputCacheStore _cacheOutStore;
        private readonly IFusionCache _cacheService;
        private readonly ISyncStatusRepository _statusRepository;
        private readonly IImageService _imageService;
        private readonly IBlobStorage _blobStorage;
        private readonly BlobStorageOptions _storageOptions;

        public SearchWordService(ILogger<SearchWordService> logger,
            IMapper mapper,
            ISearchWordRepository wordRepository,
            IOutputCacheStore cacheStore, 
            IImageService imageService, 
            IBlobStorage blobStorage,
            IOptions<BlobStorageOptions> storageOptions,
            IFusionCache fusionCache, 
            ISyncStatusRepository statusRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _wordRepository = wordRepository;
            _cacheOutStore = cacheStore;
            _imageService = imageService;
            _blobStorage = blobStorage;
            _cacheService = fusionCache;
            _statusRepository = statusRepository;
            _storageOptions = storageOptions.Value;
        }

        private string GetDefaultSearchWordImageUri(ContextType type)
        {
            return Path.Combine(_blobStorage.GetBaseUri.ToString(),
                _storageOptions.TrendContainerName,
                type.ShortName);
        }

        public async Task<SearchWordResDto> AddNewSearchWord(SearchWordAddReqDto instance, CancellationToken token)
        {
            var isDuplicate = await _wordRepository.IsDuplicate(instance.SearchWord, instance.SearchEngine, token);

            if (isDuplicate)
            {
                throw new TrendAppCoreException("Sync setting with given search word and engine needs to be unique");
            }

            var entity = _mapper.Map<SearchWord>(instance);
            entity.ImageUrl = GetDefaultSearchWordImageUri(entity.Type);
            entity.IsActive = false;
            await _wordRepository.Add(entity, token);
            
            await _cacheOutStore.EvictByTagAsync(CacheTags.SEARCH_WORD, token);
            
            return _mapper.Map<SearchWordResDto>(entity);
        }

        public async Task<SearchWordSyncDetailResDto> GetSearchWordSyncStatistic(string wordId, CancellationToken token)
        {
            var result = await _wordRepository.GetSearchWordSyncInfo(wordId, token);
            
            if (result is null)
            {
                throw new TrendNotFoundException();
            }

            var responseInst = _mapper.Map<SearchWordSyncDetailResDto>(result);

            var numberOfSyncs = await _cacheService.GetOrSetAsync(
                CacheKeys.SYNC_TOTAL_COUNT,
                _ => _statusRepository.Count(token),
                TimeSpan.FromHours(12),
                token
            );
            
            responseInst.TotalCount = numberOfSyncs;
            return responseInst;
        }

        public async Task AttachImageToSearchWord(SearchWordAttachImageReqDto req, CancellationToken token)
        {
            var instance = await _wordRepository.FindById(req.SearchWordId, token);
            if (instance is null || !instance.IsActive)
            {
                throw new TrendNotFoundException();
            }
            
            await using var stream = await _imageService.ResizeImage(req.Content, 720, 480);
            
            var uri = await _blobStorage.UploadBlobAsync(_storageOptions.TrendContainerName,
                instance.Word, 
                stream, 
                req.ContentType);

            instance.ImageUrl = uri.ToString();

            await _wordRepository.Update(instance, token);
            
            await _cacheOutStore.EvictByTagAsync(CacheTags.SEARCH_WORD, token);
        }

        public async Task ActivateSearchWord(string id, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogInformation("Search word {SearchWordId} is null or empty", id);
                throw new TrendAppCoreException("Given id is invalid");
            }

            var entity = await _wordRepository.FindById(id, token);

            if (entity is null || entity.IsActive)
            {
                _logger.LogInformation("Search word with given id {SearchWordId} not found", id);
                throw new TrendNotFoundException();
            }

            await _wordRepository.ActivateItems(new List<string> { entity.Id }, token);
            await _cacheOutStore.EvictByTagAsync(CacheTags.SEARCH_WORD, token);
        }

        public async Task<PageResponseDto<SearchWordResDto>> FilterSearchWords(SearchWordFilterReqDto req, CancellationToken token)
        {
            var search = _mapper.Map<SearchWordFilterReqQuery>(req);
            var searchResult = await _wordRepository.Filter(search, token);
            return _mapper.Map<PageResponseDto<SearchWordResDto>>(searchResult);
        }

        public async Task<List<SearchWordResDto>> GetActiveSearchWords(CancellationToken token)
        {
            var entities = await _wordRepository.GetActiveItems(token);
            var instances = _mapper.Map<List<SearchWordResDto>>(entities);
            return instances;
        }

        public async Task<List<SearchWordResDto>> GetDeactivatedSearchWords(CancellationToken token)
        {
            var entities = await _wordRepository.GetDeactivatedItems(token);
            var instances = _mapper.Map<List<SearchWordResDto>>(entities);
            return instances;
        }
        
        public async Task DeactivateSearchWord(string id, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogInformation("Search word {SearchWordId} is null or empty", id);
                throw new TrendAppCoreException("Given id is invalid");
            }

            var entity = await _wordRepository.FindById(id, token);

            if (entity is null || !entity.IsActive)
            {
                _logger.LogInformation("Item with given id {SearchWordId} not found", id);
                throw new TrendNotFoundException();
            }

            await _wordRepository.DeactivateItems(new List<string> { entity.Id }, token);
            await _cacheOutStore.EvictByTagAsync(CacheTags.SEARCH_WORD, token);
        }
    }
}
