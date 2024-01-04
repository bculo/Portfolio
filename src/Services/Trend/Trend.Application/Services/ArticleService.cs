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
        private readonly IArticleRepository _articleRepo;
        private readonly IMapper _mapper;

        public ArticleService(IArticleRepository articleRepo, IMapper mapper)
        {
            _articleRepo = articleRepo;
            _mapper = mapper;
        }

        public async Task<List<ArticleResDto>> GetLatestNewsByContextType(ContextType type, CancellationToken token)
        {
            var articles = await _articleRepo.GetActiveArticles(type, token);
            var response = _mapper.Map<List<ArticleResDto>>(articles);
            return response;
        }

        public async Task<List<ArticleResDto>> GetLatestNews(CancellationToken token)
        {
            var articles = await _articleRepo.GetActiveItems(token);
            var response = _mapper.Map<List<ArticleResDto>>(articles);
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

        public async IAsyncEnumerable<ArticleResDto> GetAllEnumerable([EnumeratorCancellation] CancellationToken token)
        {
            await foreach(var entity in _articleRepo.GetAllEnumerable(token))
            {
                yield return _mapper.Map<ArticleResDto>(entity);
            }
        }

        public async IAsyncEnumerable<ArticleResDto> GetLatestNewsEnumerable([EnumeratorCancellation] CancellationToken token)
        {
            await foreach(var entity in _articleRepo.GetActiveItemsEnumerable(token))
            {
                yield return _mapper.Map<ArticleResDto>(entity);
            }
        }

        public async IAsyncEnumerable<ArticleResDto> GetLatestNewsEnumerable(ContextType type, [EnumeratorCancellation] CancellationToken token)
        {
            await foreach (var entity in _articleRepo.GetActiveArticlesEnumerable(type, token))
            {
                yield return _mapper.Map<ArticleResDto>(entity);
            }
        }

        public async Task<PageResponseDto<ArticleResDto>> GetLatestNewsPage(ArticleFetchPageReqDto page, CancellationToken token)
        {
            throw new NotImplementedException();
            //var repoPage = await _articleRepo.FilterBy(page.Page, page.Take, i => i.Type == (ContextType)page.Type, token);
            //return _mapper.Map<PageResponseDto<ArticleDto>>(repoPage);
        }
    }
}
