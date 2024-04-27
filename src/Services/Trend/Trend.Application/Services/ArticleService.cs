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
    public class ArticleService : ServiceBase, IArticleService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ArticleService> _logger;
        private readonly IArticleRepository _articleRepo;
        private readonly IPublishEndpoint _endpoint;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IOutputCacheStore _cacheStore;

        public ArticleService(IArticleRepository articleRepo, 
            IMapper mapper, 
            ILogger<ArticleService> logger, 
            IServiceProvider provider, 
            IPublishEndpoint endpoint, 
            IDateTimeProvider timeProvider, 
            IOutputCacheStore cacheStore)
            : base(provider)
        {
            _articleRepo = articleRepo;
            _mapper = mapper;
            _logger = logger;
            _endpoint = endpoint;
            _timeProvider = timeProvider;
            _cacheStore = cacheStore;
        }
        
        public async Task<List<ArticleResDto>> GetLatestByContext(ContextType type, CancellationToken token = default)
        {
            var articles = await _articleRepo.GetActive(type, token);
            return _mapper.Map<List<ArticleResDto>>(articles);
        }

        public async Task<List<ArticleResDto>> GetLatest(CancellationToken token = default)
        {
            var articles = await _articleRepo.GetActiveItems(token);
            return _mapper.Map<List<ArticleResDto>>(articles);
        }
        
        public async Task<Either<CoreError, Unit>> Deactivate(DeactivateArticleReqDto req, CancellationToken tcs = default)
        {
            var validationResult = await Validate(req, tcs);
            if (!validationResult.IsValid)
            {
                _logger.LogInformation(LogTemplates.VALIDATION_ERROR_TEMP);
                return ArticleErrors.ValidationError(validationResult.Errors);
            }
            
            var article = await _articleRepo.FindById(req.Id, tcs);
            if (article is null)
            {
                _logger.LogInformation("Article {ArticleId} not found", req.Id);
                return ArticleErrors.NotFound;
            }
            
            await _articleRepo.DeactivateItems(req.Id.ToEnumerable(), tcs);
            await _cacheStore.EvictByTagAsync(CacheTags.NEWS, tcs);
            await _endpoint.Publish(new ArticleDeactivated
            {
                Time = _timeProvider.Utc,
                ArticleId = req.Id
            }, tcs);
            
            return Unit.Default;
        }

        public async Task<Either<CoreError, Unit>> Activate(ActivateArticleReqDto req, CancellationToken tcs = default)
        {
            var validationResult = await Validate(req, tcs);
            if (!validationResult.IsValid)
            {
                _logger.LogInformation(LogTemplates.VALIDATION_ERROR_TEMP);
                return ArticleErrors.ValidationError(validationResult.Errors);
            }
            
            var article = await _articleRepo.FindById(req.Id, tcs);
            if (article is null)
            {
                _logger.LogInformation("Article {ArticleId} not found", req.Id);
                return ArticleErrors.NotFound;
            }
            
            await _articleRepo.ActivateItems(req.Id.ToEnumerable(), tcs);
            await _cacheStore.EvictByTagAsync(CacheTags.NEWS, tcs);
            await _endpoint.Publish(new ArticleActivated
            {
                Time = _timeProvider.Utc,
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
                _logger.LogInformation(LogTemplates.VALIDATION_ERROR_TEMP);
                return ArticleErrors.ValidationError(validationResult.Errors);
            }
            
            var search = _mapper.Map<FilterArticlesReqQuery>(req);
            var searchResult = await _articleRepo.Filter(search, token);
            return _mapper.Map<PageResponseDto<ArticleResDto>>(searchResult);
        }
    }
}
