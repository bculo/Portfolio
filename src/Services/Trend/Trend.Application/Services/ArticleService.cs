using AutoMapper;
using Dtos.Common;
using Events.Common.Trend;
using LanguageExt;
using MassTransit;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Logging;
using Time.Abstract.Contracts;
using Trend.Application.Configurations.Constants;
using Trend.Application.Extensions;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models;
using Trend.Domain.Enums;
using Trend.Domain.Errors;

namespace Trend.Application.Services
{
    public class ArticleService(
        IArticleRepository articleRepo,
        IMapper mapper,
        ILogger<ArticleService> logger,
        IServiceProvider provider,
        IPublishEndpoint endpoint,
        IDateTimeProvider timeProvider,
        IOutputCacheStore cacheStore)
        : ServiceBase(provider), IArticleService
    {
        public async Task<List<ArticleResDto>> GetLatestByContext(ContextType type, CancellationToken token = default)
        {
            var articles = await articleRepo.GetActive(type, token);
            return mapper.Map<List<ArticleResDto>>(articles);
        }

        public async Task<List<ArticleResDto>> GetLatest(CancellationToken token = default)
        {
            var articles = await articleRepo.GetActiveItems(token);
            return mapper.Map<List<ArticleResDto>>(articles);
        }
        
        public async Task<Either<CoreError, Unit>> Deactivate(DeactivateArticleReqDto req, CancellationToken tcs = default)
        {
            var validationResult = await Validate(req, tcs);
            if (!validationResult.IsValid)
            {
                logger.LogInformation(LogTemplates.ValidationErrorTemp);
                return ArticleErrors.ValidationError(validationResult.Errors);
            }
            
            var article = await articleRepo.FindById(req.Id, tcs);
            if (article is null)
            {
                logger.LogInformation("Article {ArticleId} not found", req.Id);
                return ArticleErrors.NotFound;
            }
            
            await articleRepo.DeactivateItems(req.Id.ToEnumerable(), tcs);
            await cacheStore.EvictByTagAsync(CacheTags.News, tcs);
            await endpoint.Publish(new ArticleDeactivated
            {
                Time = timeProvider.Utc,
                ArticleId = req.Id
            }, tcs);
            
            return Unit.Default;
        }

        public async Task<Either<CoreError, Unit>> Activate(ActivateArticleReqDto req, CancellationToken tcs = default)
        {
            var validationResult = await Validate(req, tcs);
            if (!validationResult.IsValid)
            {
                logger.LogInformation(LogTemplates.ValidationErrorTemp);
                return ArticleErrors.ValidationError(validationResult.Errors);
            }
            
            var article = await articleRepo.FindById(req.Id, tcs);
            if (article is null)
            {
                logger.LogInformation("Article {ArticleId} not found", req.Id);
                return ArticleErrors.NotFound;
            }
            
            await articleRepo.ActivateItems(req.Id.ToEnumerable(), tcs);
            await cacheStore.EvictByTagAsync(CacheTags.News, tcs);
            await endpoint.Publish(new ArticleActivated
            {
                Time = timeProvider.Utc,
                ArticleId = req.Id
            }, tcs);
            
            return Unit.Default;
        }

        public async Task<Either<CoreError, PageResponseDto<ArticleResDto>>> Filter(
            FilterArticlesReqDto req, 
            CancellationToken token = default)
        {
            var validationResult = await Validate(req, token);
            if (!validationResult.IsValid)
            {
                logger.LogInformation(LogTemplates.ValidationErrorTemp);
                return ArticleErrors.ValidationError(validationResult.Errors);
            }
            
            var search = mapper.Map<FilterArticlesReqQuery>(req);
            var searchResult = await articleRepo.Filter(search, token);
            return mapper.Map<PageResponseDto<ArticleResDto>>(searchResult);
        }
    }
}
