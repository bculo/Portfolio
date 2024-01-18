using System.Runtime.CompilerServices;
using AutoMapper;
using Dtos.Common;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models.Dtos;
using Trend.Domain.Enums;
using Trend.Domain.Errors;
using Trend.Domain.Exceptions;

namespace Trend.Application.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ArticleService> _logger;
        private readonly IArticleRepository _articleRepo;

        public ArticleService(IArticleRepository articleRepo, 
            IMapper mapper, 
            ILogger<ArticleService> logger)
        {
            _articleRepo = articleRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<ArticleResDto>> GetLatestNewsByContextType(ContextType type, CancellationToken token = default)
        {
            var articles = await _articleRepo.GetActiveArticles(type, token);
            var response = _mapper.Map<List<ArticleResDto>>(articles);
            return response;
        }

        public async Task<List<ArticleResDto>> GetLatestNews(CancellationToken token = default)
        {
            var articles = await _articleRepo.GetActiveItems(token);
            var response = _mapper.Map<List<ArticleResDto>>(articles);
            return response;
        }

        public async Task<Either<CoreError, Unit>> Deactivate(string articleId, CancellationToken tcs = default)
        {
            if (string.IsNullOrWhiteSpace(articleId))
            {
                _logger.LogInformation("Search word is null or empty");
                return ArticleErrors.EmptyId;
            }
            
            var article = await _articleRepo.FindById(articleId, tcs);
            if (article is null)
            {
                _logger.LogInformation("Article {ArticleId} not found", articleId);
                return ArticleErrors.NotFound;
            }
            
            await _articleRepo.DeactivateItems(new List<string> { articleId }, tcs);
            return Unit.Default;
        }

        public async Task<Either<CoreError, Unit>> Activate(string articleId, CancellationToken tcs = default)
        {
            if (string.IsNullOrWhiteSpace(articleId))
            {
                _logger.LogInformation("Search word is null or empty");
                return ArticleErrors.EmptyId;
            }
            
            var article = await _articleRepo.FindById(articleId, tcs);
            if (article is null)
            {
                _logger.LogInformation("Article {ArticleId} not found", articleId);
                throw new TrendNotFoundException("Article not found");
            }
            
            await _articleRepo.ActivateItems(new List<string> { articleId }, tcs);
            return Unit.Default;
        }

        public async Task<PageResponseDto<ArticleResDto>> GetLatestNewsPage(ArticleFetchPageReqDto page, CancellationToken token)
        {
            throw new NotImplementedException();
            //var repoPage = await _articleRepo.FilterBy(page.Page, page.Take, i => i.Type == (ContextType)page.Type, token);
            //return _mapper.Map<PageResponseDto<ArticleDto>>(repoPage);
        }
    }
}
