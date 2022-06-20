using AutoMapper;
using Dtos.Common.Shared;
using Dtos.Common.v1.Trend;
using Microsoft.Extensions.Logging;
using System.Linq;
using Trend.Application.Interfaces;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Interfaces;
using Trend.Domain.Queries.Requests.Common;

namespace Trend.Application.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ILogger<ArticleService> _logger;
        private readonly IArticleRepository _articleRepo;
        private readonly ISyncStatusRepository _syncRepo;
        private readonly IMapper _mapper;

        public ArticleService(ILogger<ArticleService> logger,
            IArticleRepository articleRepo,
            ISyncStatusRepository syncRepo,
            IMapper mapper)
        {
            _logger = logger;
            _articleRepo = articleRepo;
            _syncRepo = syncRepo;
            _mapper = mapper;
        }

        public async Task<List<ArticleDto>> GetLatestNews(ContextType type)
        {
            _logger.LogTrace("Fetching news for type {0}", type);

            var articles = await _articleRepo.GetActiveArticles(type);

            _logger.LogTrace("Fetched records from database");

            var dtos = _mapper.Map<List<ArticleDto>>(articles);

            _logger.LogTrace("Entities mapped to dtos");

            return dtos;
        }

        public async Task<List<ArticleTypeDto>> GetLatestNews()
        {
            _logger.LogTrace("Fetching latest news");

            var articles = await _articleRepo.GetActiveArticles();

            _logger.LogTrace("Fetched records from database");

            var dtos = _mapper.Map<List<ArticleTypeDto>>(articles);

            _logger.LogTrace("Entities mapped to dtos");

            return dtos;
        }

        public async IAsyncEnumerable<ArticleTypeDto> GetAllEnumerable()
        {
            _logger.LogTrace("Fetching entities");

            await foreach(var entity in _articleRepo.GetAllEnumerable())
            {
                yield return _mapper.Map<ArticleTypeDto>(entity);
            }
        }

        public async IAsyncEnumerable<ArticleTypeDto> GetLatestNewsEnumerable()
        {
            _logger.LogTrace("Fetching latest news");

            await foreach(var entity in _articleRepo.GetActiveArticlesEnumerable())
            {
                yield return _mapper.Map<ArticleTypeDto>(entity);
            }
        }

        public async IAsyncEnumerable<ArticleDto> GetLatestNewsEnumerable(ContextType type)
        {
            _logger.LogTrace("Fetching latest news");

            await foreach (var entity in _articleRepo.GetActiveArticlesEnumerable(type))
            {
                yield return _mapper.Map<ArticleDto>(entity);
            }
        }

        public async Task<PageResponseDto<ArticleDto>> GetLatestNewsPage(FetchArticleTypePageDto page)
        {
            _logger.LogTrace("Fetching articles page");

            var repoPage = await _articleRepo.FilterBy(page.Page, page.Take, i => i.Type == (ContextType)page.Type);

            _logger.LogTrace("Mapping entities to dto instance");

            return _mapper.Map<PageResponseDto<ArticleDto>>(repoPage);
        }

        public async Task<PageResponseDto<ArticleTypeDto>> GetLatestNewsPage(FetchLatestNewsPageDto page)
        {
            _logger.LogTrace("Fetching articles page");

            var repoPage = await _articleRepo.FilterBy(page.Page, page.Take);

            _logger.LogTrace("Mapping entities to dto instance");

            return _mapper.Map<PageResponseDto<ArticleTypeDto>>(repoPage);
        }
    }
}
