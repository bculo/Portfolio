using AutoMapper;
using Dtos.Common;
using LanguageExt;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Time.Abstract.Contracts;
using Trend.Application.Configurations.Constants;
using Trend.Application.Configurations.Options;
using Trend.Application.Extensions;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Errors;
using ZiggyCreatures.Caching.Fusion;

namespace Trend.Application.Services
{
    public class SearchWordService : ServiceBase, ISearchWordService
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
        private readonly IDateTimeProvider _time;

        public SearchWordService(ILogger<SearchWordService> logger,
            IMapper mapper,
            ISearchWordRepository wordRepository,
            IOutputCacheStore cacheStore, 
            IImageService imageService, 
            IBlobStorage blobStorage,
            IOptions<BlobStorageOptions> storageOptions,
            IFusionCache fusionCache, 
            ISyncStatusRepository statusRepository, 
            IDateTimeProvider time,
            IServiceProvider provider)
            : base(provider)
        {
            _logger = logger;
            _mapper = mapper;
            _wordRepository = wordRepository;
            _cacheOutStore = cacheStore;
            _imageService = imageService;
            _blobStorage = blobStorage;
            _cacheService = fusionCache;
            _statusRepository = statusRepository;
            _time = time;
            _storageOptions = storageOptions.Value;
        }

        private string GetDefaultSearchWordImageUri(ContextType type)
        {
            return Path.Combine(_blobStorage.GetBaseUri.ToString(),
                _storageOptions.TrendContainerName,
                type.ShortName);
        }

        public async Task<Either<CoreError, SearchWordResDto>> CreateNew(
            AddWordReqDto instance, 
            CancellationToken token = default)
        {
            var validationResult = await Validate(instance, token);
            if (!validationResult.IsValid)
            {
                _logger.LogInformation(LogTemplates.VALIDATION_ERROR_TEMP);
                return SearchWordErrors.ValidationError(validationResult.Errors);
            }
            
            var entity = _mapper.Map<SearchWord>(instance);
            entity.ImageUrl = GetDefaultSearchWordImageUri(entity.Type);
            entity.IsActive = false;
            entity.Created = _time.Now;
            
            await _wordRepository.Add(entity, token);
            await _cacheOutStore.EvictByTagAsync(CacheTags.SEARCH_WORD, token);

            return _mapper.Map<SearchWordResDto>(entity);
        }

        public async Task<Either<CoreError, SearchWordSyncStatisticResDto>> GetSyncStatistic(
            SearchWordSyncStatisticReqDto req, 
            CancellationToken token = default)
        {
            var validationResult = await Validate(req, token);
            if (!validationResult.IsValid)
            {
                _logger.LogInformation(LogTemplates.VALIDATION_ERROR_TEMP);
                return SearchWordErrors.ValidationError(validationResult.Errors);
            }
            
            var result = await _wordRepository.GetSyncStatisticInfo(req.Id, token);
            if (result is null)
            {
                _logger.LogInformation("Search word {SearchWordId} not found", req.Id);
                return SearchWordErrors.NotFound;
            }
            
            var numberOfSyncs = await _cacheService.GetOrSetAsync(
                CacheKeys.SYNC_TOTAL_COUNT,
                _ => _statusRepository.Count(token),
                opt => opt
                    .SetDuration(TimeSpan.FromHours(12))
                    .SetFailSafe(true, TimeSpan.FromHours(13))
                    .SetFactoryTimeouts(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(1500)),
                token
            );
            
            var responseInst = _mapper.Map<SearchWordSyncStatisticResDto>(result);
            responseInst.TotalCount = numberOfSyncs;
            
            return responseInst;
        }

        public async Task<Either<CoreError, Unit>> AttachImage(
            AttachImageToSearchWordReqDto req, 
            CancellationToken token = default)
        {
            var validationResult = await Validate(req, token);
            if (!validationResult.IsValid)
            {
                _logger.LogInformation(LogTemplates.VALIDATION_ERROR_TEMP);
                return SearchWordErrors.ValidationError(validationResult.Errors);
            }
            
            var instance = await _wordRepository.FindById(req.Id, token);
            if (instance is null)
            {
                _logger.LogInformation("Search word {SearchWordId} not found", req.Id);
                return SearchWordErrors.NotFound;
            }
            
            await using var stream = await _imageService.ResizeImage(req.Content, 720, 480, token);
            instance.ImageUrl = (await _blobStorage.Upload(_storageOptions.TrendContainerName,
                instance.Word, 
                stream, 
                req.ContentType, token)).ToString();
            
            await _wordRepository.Update(instance, token);
            await _cacheOutStore.EvictByTagAsync(CacheTags.SEARCH_WORD, token);
            return Unit.Default;
        }

        public async Task<Either<CoreError, Unit>> Activate(
            ActivateSearchWordReqDto req, 
            CancellationToken token)
        {
            var validationResult = await Validate(req, token);
            if (!validationResult.IsValid)
            {
                _logger.LogInformation(LogTemplates.VALIDATION_ERROR_TEMP);
                return SearchWordErrors.ValidationError(validationResult.Errors);
            }
            
            var entity = await _wordRepository.FindById(req.Id, token);
            if (entity is null || entity.IsActive)
            {
                _logger.LogInformation("Item with given id {SearchWordId} not found", req.Id);
                return SearchWordErrors.NotFound;
            }

            await _wordRepository.ActivateItems(req.Id.ToEnumerable(), token);
            await _cacheOutStore.EvictByTagAsync(CacheTags.SEARCH_WORD, token);
            return Unit.Default;
        }

        public async Task<Either<CoreError, PageResponseDto<SearchWordResDto>>> Filter(
            FilterSearchWordsReqDto req, 
            CancellationToken token = default)
        {
            var validationResult = await Validate(req, token);
            if (!validationResult.IsValid)
            {
                _logger.LogInformation(LogTemplates.VALIDATION_ERROR_TEMP);
                return SearchWordErrors.ValidationError(validationResult.Errors);
            }
            
            var search = _mapper.Map<SearchWordFilterReqQuery>(req);
            var searchResult = await _wordRepository.Filter(search, token);
            return _mapper.Map<PageResponseDto<SearchWordResDto>>(searchResult);
        }

        public async Task<List<SearchWordResDto>> GetActiveItems(CancellationToken token = default)
        {
            var entities = await _wordRepository.GetActiveItems(token);
            return _mapper.Map<List<SearchWordResDto>>(entities);
        }

        public async Task<List<SearchWordResDto>> GetDeactivatedItems(CancellationToken token = default)
        {
            var entities = await _wordRepository.GetDeactivatedItems(token);
            return _mapper.Map<List<SearchWordResDto>>(entities);
        }
        
        public async Task<Either<CoreError, Unit>> Deactivate(
            DeactivateSearchWordReqDto req, 
            CancellationToken token = default)
        {
            var validationResult = await Validate(req, token);
            if (!validationResult.IsValid)
            {
                _logger.LogInformation(LogTemplates.VALIDATION_ERROR_TEMP);
                return SearchWordErrors.ValidationError(validationResult.Errors);
            }

            var entity = await _wordRepository.FindById(req.Id, token);
            if (entity is null || !entity.IsActive)
            {
                _logger.LogInformation("Item with given id {SearchWordId} not found", req.Id);
                return SearchWordErrors.NotFound;
            }

            await _wordRepository.DeactivateItems(req.Id.ToEnumerable(), token);
            await _cacheOutStore.EvictByTagAsync(CacheTags.SEARCH_WORD, token);
            return Unit.Default;
        }
    }
}
