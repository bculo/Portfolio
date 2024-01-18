using System.Runtime.CompilerServices;
using AutoMapper;
using Dtos.Common;
using Trend.Application.Interfaces;
using Trend.Application.Interfaces.Models.Dtos;
using Trend.Domain.Enums;
using Trend.Domain.Exceptions;

namespace Trend.Application.Services
{
    public class ArticleService : IArticleService
    {
        protected readonly IArticleRepository _articleRepo;
        protected readonly IMapper _mapper;

        public ArticleService(IArticleRepository articleRepo, IMapper mapper)
        {
            _articleRepo = articleRepo;
            _mapper = mapper;
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

        public async Task Deactivate(string articleId, CancellationToken tcs = default)
        {
            var article = await _articleRepo.FindById(articleId, tcs);
            if (article is null)
            {
                throw new TrendNotFoundException("Article not found");
            }
            
            await _articleRepo.DeactivateItems(new List<string> { articleId }, tcs);
        }

        public async Task Activate(string articleId, CancellationToken tcs = default)
        {
            var article = await _articleRepo.FindById(articleId, tcs);
            if (article is null)
            {
                throw new TrendNotFoundException("Article not found");
            }
            
            await _articleRepo.ActivateItems(new List<string> { articleId }, tcs);
        }

        public async Task<PageResponseDto<ArticleResDto>> GetLatestNewsPage(ArticleFetchPageReqDto page, CancellationToken token)
        {
            throw new NotImplementedException();
            //var repoPage = await _articleRepo.FilterBy(page.Page, page.Take, i => i.Type == (ContextType)page.Type, token);
            //return _mapper.Map<PageResponseDto<ArticleDto>>(repoPage);
        }
    }
}
