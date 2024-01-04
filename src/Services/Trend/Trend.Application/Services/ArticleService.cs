using System.Runtime.CompilerServices;
using AutoMapper;
using Dtos.Common.Shared;
using Dtos.Common.v1.Trend;
using Dtos.Common.v1.Trend.Article;
using Trend.Application.Interfaces;
using Trend.Domain.Enums;
using Trend.Domain.Exceptions;
using Trend.Domain.Interfaces;

namespace Trend.Application.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepo;
        private readonly IMapper _mapper;

        public ArticleService(IArticleRepository articleRepo, IMapper mapper)
        {
            _articleRepo = articleRepo;
            _mapper = mapper;
        }

        public async Task<List<ArticleDto>> GetLatestNewsByContextType(ContextType type, CancellationToken token)
        {
            var articles = await _articleRepo.GetActiveArticles(type, token);
            var response = _mapper.Map<List<ArticleDto>>(articles);
            return response;
        }

        public async Task<List<ArticleTypeDto>> GetLatestNews(CancellationToken token)
        {
            var articles = await _articleRepo.GetActiveItems(token);
            var response = _mapper.Map<List<ArticleTypeDto>>(articles);
            return response;
        }

        public async Task Deactivate(string articleId, CancellationToken tcs)
        {
            var article = await _articleRepo.FindById(articleId, tcs);
            if (article is null)
            {
                throw new TrendNotFoundException("Article not found");
            }
            await _articleRepo.DeactivateItems(new List<string> { articleId }, tcs);
        }

        public async IAsyncEnumerable<ArticleTypeDto> GetAllEnumerable([EnumeratorCancellation] CancellationToken token)
        {
            await foreach(var entity in _articleRepo.GetAllEnumerable(token))
            {
                yield return _mapper.Map<ArticleTypeDto>(entity);
            }
        }

        public async IAsyncEnumerable<ArticleTypeDto> GetLatestNewsEnumerable([EnumeratorCancellation] CancellationToken token)
        {
            await foreach(var entity in _articleRepo.GetActiveItemsEnumerable(token))
            {
                yield return _mapper.Map<ArticleTypeDto>(entity);
            }
        }

        public async IAsyncEnumerable<ArticleDto> GetLatestNewsEnumerable(ContextType type, [EnumeratorCancellation] CancellationToken token)
        {
            await foreach (var entity in _articleRepo.GetActiveArticlesEnumerable(type, token))
            {
                yield return _mapper.Map<ArticleDto>(entity);
            }
        }

        public async Task<PageResponseDto<ArticleDto>> GetLatestNewsPage(FetchArticleTypePageDto page, CancellationToken token)
        {
            var repoPage = await _articleRepo.FilterBy(page.Page, page.Take, i => i.Type == (ContextType)page.Type, token);
            return _mapper.Map<PageResponseDto<ArticleDto>>(repoPage);
        }

        public async Task<PageResponseDto<ArticleTypeDto>> GetLatestNewsPage(FetchLatestNewsPageDto page, CancellationToken token)
        {
            var repoPage = await _articleRepo.FilterBy(page.Page, page.Take, null, token);
            return _mapper.Map<PageResponseDto<ArticleTypeDto>>(repoPage);
        }
    }
}
