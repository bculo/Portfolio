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
using Trend.Domain.Exceptions;
using ZiggyCreatures.Caching.Fusion;

namespace Trend.Application.Services
{
    public class SearchWordService(
        ILogger<SearchWordService> logger,
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
        : ServiceBase(provider), ISearchWordService
    {
        private readonly BlobStorageOptions _storageOptions = storageOptions.Value;

        private string GetDefaultSearchWordImageUri(ContextType type)
        {
            var imageName= type.Value switch
            {
                0 => "Crypto",
                1 => "Stock",
                2 => "Forex",
                _ => throw new TrendAppCoreException("Not supported exception")
            };

            return Path.Combine(blobStorage.GetBaseUri.ToString(),
                _storageOptions.TrendContainerName,
                imageName);
        }

        public async Task<Either<CoreError, SearchWordResDto>> CreateNew(
            AddWordReqDto instance, 
            CancellationToken token = default)
        {
            var validationResult = await Validate(instance, token);
            if (!validationResult.IsValid)
            {
                logger.LogInformation(LogTemplates.ValidationErrorTemp);
                return SearchWordErrors.ValidationError(validationResult.Errors);
            }
            
            var entity = mapper.Map<SearchWord>(instance);
            entity.ImageUrl = GetDefaultSearchWordImageUri(entity.Type);
            entity.IsActive = false;
            entity.Created = time.Time;
            
            await wordRepository.Add(entity, token);
            await cacheStore.EvictByTagAsync(CacheTags.SearchWord, token);

            return mapper.Map<SearchWordResDto>(entity);
        }

        public async Task<Either<CoreError, SearchWordSyncStatisticResDto>> GetSyncStatistic(
            SearchWordSyncStatisticReqDto req, 
            CancellationToken token = default)
        {
            var validationResult = await Validate(req, token);
            if (!validationResult.IsValid)
            {
                logger.LogInformation(LogTemplates.ValidationErrorTemp);
                return SearchWordErrors.ValidationError(validationResult.Errors);
            }
            
            var result = await wordRepository.GetSyncStatisticInfo(req.Id, token);
            if (result is null)
            {
                logger.LogInformation("Search word {SearchWordId} not found", req.Id);
                return SearchWordErrors.NotFound;
            }
            
            var numberOfSyncs = await fusionCache.GetOrSetAsync(
                CacheKeys.SyncTotalCount,
                _ => statusRepository.Count(token),
                opt => opt
                    .SetDuration(TimeSpan.FromHours(12))
                    .SetFailSafe(true, TimeSpan.FromHours(13))
                    .SetFactoryTimeouts(TimeSpan.FromMilliseconds(200), TimeSpan.FromMilliseconds(1500)),
                token
            );
            
            var responseInst = mapper.Map<SearchWordSyncStatisticResDto>(result);
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
                logger.LogInformation(LogTemplates.ValidationErrorTemp);
                return SearchWordErrors.ValidationError(validationResult.Errors);
            }
            
            var instance = await wordRepository.FindById(req.Id, token);
            if (instance is null)
            {
                logger.LogInformation("Search word {SearchWordId} not found", req.Id);
                return SearchWordErrors.NotFound;
            }
            
            await using var stream = await imageService.ResizeImage(req.Content, 720, 480, token);
            instance.ImageUrl = (await blobStorage.Upload(_storageOptions.TrendContainerName,
                instance.Word, 
                stream, 
                req.ContentType, token)).ToString();
            
            await wordRepository.Update(instance, token);
            await cacheStore.EvictByTagAsync(CacheTags.SearchWord, token);
            return Unit.Default;
        }

        public async Task<Either<CoreError, Unit>> Activate(
            ActivateSearchWordReqDto req, 
            CancellationToken token)
        {
            var validationResult = await Validate(req, token);
            if (!validationResult.IsValid)
            {
                logger.LogInformation(LogTemplates.ValidationErrorTemp);
                return SearchWordErrors.ValidationError(validationResult.Errors);
            }
            
            var entity = await wordRepository.FindById(req.Id, token);
            if (entity is null || entity.IsActive)
            {
                logger.LogInformation("Item with given id {SearchWordId} not found", req.Id);
                return SearchWordErrors.NotFound;
            }

            await wordRepository.ActivateItems(req.Id.ToEnumerable(), token);
            await cacheStore.EvictByTagAsync(CacheTags.SearchWord, token);
            return Unit.Default;
        }

        public async Task<Either<CoreError, PageResponseDto<SearchWordResDto>>> Filter(
            FilterSearchWordsReqDto req, 
            CancellationToken token = default)
        {
            var validationResult = await Validate(req, token);
            if (!validationResult.IsValid)
            {
                logger.LogInformation(LogTemplates.ValidationErrorTemp);
                return SearchWordErrors.ValidationError(validationResult.Errors);
            }
            
            var search = mapper.Map<SearchWordFilterReqQuery>(req);
            var searchResult = await wordRepository.Filter(search, token);
            return mapper.Map<PageResponseDto<SearchWordResDto>>(searchResult);
        }

        public async Task<List<SearchWordResDto>> GetActiveItems(CancellationToken token = default)
        {
            var entities = await wordRepository.GetActiveItems(token);
            return mapper.Map<List<SearchWordResDto>>(entities);
        }

        public async Task<List<SearchWordResDto>> GetDeactivatedItems(CancellationToken token = default)
        {
            var entities = await wordRepository.GetDeactivatedItems(token);
            return mapper.Map<List<SearchWordResDto>>(entities);
        }
        
        public async Task<Either<CoreError, Unit>> Deactivate(
            DeactivateSearchWordReqDto req, 
            CancellationToken token = default)
        {
            var validationResult = await Validate(req, token);
            if (!validationResult.IsValid)
            {
                logger.LogInformation(LogTemplates.ValidationErrorTemp);
                return SearchWordErrors.ValidationError(validationResult.Errors);
            }

            var entity = await wordRepository.FindById(req.Id, token);
            if (entity is null || !entity.IsActive)
            {
                logger.LogInformation("Item with given id {SearchWordId} not found", req.Id);
                return SearchWordErrors.NotFound;
            }

            await wordRepository.DeactivateItems(req.Id.ToEnumerable(), token);
            await cacheStore.EvictByTagAsync(CacheTags.SearchWord, token);
            return Unit.Default;
        }
    }
}
