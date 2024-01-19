using AutoMapper;
using Dtos.Common;
using LanguageExt;
using Microsoft.Extensions.Logging;
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

        public ArticleService(IArticleRepository articleRepo, 
            IMapper mapper, 
            ILogger<ArticleService> logger, 
            IServiceProvider provider)
            : base(provider)
        {
            _articleRepo = articleRepo;
            _mapper = mapper;
            _logger = logger;
        }
        
        public async Task<List<ArticleResDto>> GetLatestNewsByContextType(ContextType type, CancellationToken token = default)
        {
            var articles = await _articleRepo.GetActiveArticles(type, token);
            return _mapper.Map<List<ArticleResDto>>(articles);
        }

        public async Task<List<ArticleResDto>> GetLatestNews(CancellationToken token = default)
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
            
            var article = await _articleRepo.FindById(req.ArticleId, tcs);
            if (article is null)
            {
                _logger.LogInformation("Article {ArticleId} not found", req.ArticleId);
                return ArticleErrors.NotFound;
            }
            
            await _articleRepo.DeactivateItems(req.ArticleId.ToEnumerable(), tcs);
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
            
            var article = await _articleRepo.FindById(req.ArticleId, tcs);
            if (article is null)
            {
                _logger.LogInformation("Article {ArticleId} not found", req.ArticleId);
                return ArticleErrors.NotFound;
            }
            
            await _articleRepo.ActivateItems(req.ArticleId.ToEnumerable(), tcs);
            return Unit.Default;
        }

        public async Task<PageResponseDto<ArticleResDto>> GetLatestNewsPage(FetchArticlePageReqDto page, CancellationToken token)
        {
            throw new NotImplementedException();
            //var repoPage = await _articleRepo.FilterBy(page.Page, page.Take, i => i.Type == (ContextType)page.Type, token);
            //return _mapper.Map<PageResponseDto<ArticleDto>>(repoPage);
        }
    }
}
