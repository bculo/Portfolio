using AutoMapper;
using Dtos.Common;
using LanguageExt;
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
using Trend.Domain.Errors;
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

        public async Task<Either<TrendError, SearchWordResDto>> AddNewSearchWord(
            SearchWordAddReqDto instance, 
            CancellationToken token = default)
        {
            var isDuplicate = await _wordRepository.IsDuplicate(instance.SearchWord, instance.SearchEngine, token);
            if (isDuplicate)
            {
                _logger.LogInformation("Search word {SearchWord} already exists", instance.SearchWord);
                return SearchWordErrors.Exists;
            }

            var entity = _mapper.Map<SearchWord>(instance);
            entity.ImageUrl = GetDefaultSearchWordImageUri(entity.Type);
            entity.IsActive = false;
            
            await _wordRepository.Add(entity, token);
            await _cacheOutStore.EvictByTagAsync(CacheTags.SEARCH_WORD, token);

            var response = _mapper.Map<SearchWordResDto>(entity);
            return response;
        }

        public async Task<Either<TrendError, SearchWordSyncDetailResDto>> GetSearchWordSyncStatistic(
            string wordId, 
            CancellationToken token = default)
        {
            var result = await _wordRepository.GetSearchWordSyncInfo(wordId, token);
            if (result is null)
            {
                _logger.LogInformation("Search word {SearchWordId} not found", wordId);
                return SearchWordErrors.NotFound;
            }
            
            var numberOfSyncs = await _cacheService.GetOrSetAsync(
                CacheKeys.SYNC_TOTAL_COUNT,
                _ => _statusRepository.Count(token),
                TimeSpan.FromHours(12),
                token
            );
            
            var responseInst = _mapper.Map<SearchWordSyncDetailResDto>(result);
            responseInst.TotalCount = numberOfSyncs;
            
            return responseInst;
        }

        public async Task<Either<TrendError, Unit>> AttachImageToSearchWord(SearchWordAttachImageReqDto req, 
            CancellationToken token = default)
        {
            var instance = await _wordRepository.FindById(req.SearchWordId, token);
            if (instance is null)
            {
                _logger.LogInformation("Search word {SearchWordId} not found", req.SearchWordId);
                return SearchWordErrors.NotFound;
            }
            
            await using var stream = await _imageService.ResizeImage(req.Content, 720, 480);
            instance.ImageUrl = (await _blobStorage.UploadBlobAsync(_storageOptions.TrendContainerName,
                instance.Word, 
                stream, 
                req.ContentType))?.ToString();
            
            await _wordRepository.Update(instance, token);
            await _cacheOutStore.EvictByTagAsync(CacheTags.SEARCH_WORD, token);
            return Unit.Default;
        }

        public async Task<Either<TrendError, Unit>> ActivateSearchWord(string id, 
            CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogInformation("Search word is null or empty");
                return SearchWordErrors.EmptyId;
            }

            var entity = await _wordRepository.FindById(id, token);
            if (entity is null || entity.IsActive)
            {
                _logger.LogInformation("Item with given id {SearchWordId} not found", id);
                return SearchWordErrors.NotFound;
            }

            await _wordRepository.ActivateItems(new List<string> { entity.Id }, token);
            await _cacheOutStore.EvictByTagAsync(CacheTags.SEARCH_WORD, token);
            return Unit.Default;
        }

        public async Task<PageResponseDto<SearchWordResDto>> FilterSearchWords(SearchWordFilterReqDto req, 
            CancellationToken token = default)
        {
            var search = _mapper.Map<SearchWordFilterReqQuery>(req);
            var searchResult = await _wordRepository.Filter(search, token);
            return _mapper.Map<PageResponseDto<SearchWordResDto>>(searchResult);
        }

        public async Task<List<SearchWordResDto>> GetActiveSearchWords(CancellationToken token = default)
        {
            var entities = await _wordRepository.GetActiveItems(token);
            var instances = _mapper.Map<List<SearchWordResDto>>(entities);
            return instances;
        }

        public async Task<List<SearchWordResDto>> GetDeactivatedSearchWords(CancellationToken token = default)
        {
            var entities = await _wordRepository.GetDeactivatedItems(token);
            var instances = _mapper.Map<List<SearchWordResDto>>(entities);
            return instances;
        }
        
        public async Task<Either<TrendError, Unit>> DeactivateSearchWord(string id, CancellationToken token = default)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogInformation("Search word is null or empty");
                return SearchWordErrors.EmptyId;
            }

            var entity = await _wordRepository.FindById(id, token);
            if (entity is null || !entity.IsActive)
            {
                _logger.LogInformation("Item with given id {SearchWordId} not found", id);
                return SearchWordErrors.NotFound;
            }

            await _wordRepository.DeactivateItems(new List<string> { entity.Id }, token);
            await _cacheOutStore.EvictByTagAsync(CacheTags.SEARCH_WORD, token);
            return Unit.Default;
        }
    }
}
