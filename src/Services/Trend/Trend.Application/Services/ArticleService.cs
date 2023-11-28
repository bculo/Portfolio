using AutoMapper;
using Dtos.Common.Shared;
using Dtos.Common.v1.Trend;
using Dtos.Common.v1.Trend.Article;
using Microsoft.AspNetCore.OutputCaching;
using Trend.Application.Interfaces;
using Trend.Domain.Enums;
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

        public async Task<List<ArticleDto>> GetLatestNews(ContextType type)
        {
            var articles = await _articleRepo.GetActiveArticles(type);
            var dtos = _mapper.Map<List<ArticleDto>>(articles);
            return dtos;
        }

        public async Task<List<ArticleTypeDto>> GetLatestNews()
        {
            var articles = await _articleRepo.GetActiveArticles();
            var dtos = _mapper.Map<List<ArticleTypeDto>>(articles);
            return dtos;
        }

        public async IAsyncEnumerable<ArticleTypeDto> GetAllEnumerable()
        {
            await foreach(var entity in _articleRepo.GetAllEnumerable())
            {
                yield return _mapper.Map<ArticleTypeDto>(entity);
            }
        }

        public async IAsyncEnumerable<ArticleTypeDto> GetLatestNewsEnumerable()
        {
            await foreach(var entity in _articleRepo.GetActiveArticlesEnumerable())
            {
                yield return _mapper.Map<ArticleTypeDto>(entity);
            }
        }

        public async IAsyncEnumerable<ArticleDto> GetLatestNewsEnumerable(ContextType type)
        {
            await foreach (var entity in _articleRepo.GetActiveArticlesEnumerable(type))
            {
                yield return _mapper.Map<ArticleDto>(entity);
            }
        }

        public async Task<PageResponseDto<ArticleDto>> GetLatestNewsPage(FetchArticleTypePageDto page)
        {
            var repoPage = await _articleRepo.FilterBy(page.Page, page.Take, i => i.Type == (ContextType)page.Type);
            return _mapper.Map<PageResponseDto<ArticleDto>>(repoPage);
        }

        public async Task<PageResponseDto<ArticleTypeDto>> GetLatestNewsPage(FetchLatestNewsPageDto page)
        {
            var repoPage = await _articleRepo.FilterBy(page.Page, page.Take);
            return _mapper.Map<PageResponseDto<ArticleTypeDto>>(repoPage);
        }
    }
}
