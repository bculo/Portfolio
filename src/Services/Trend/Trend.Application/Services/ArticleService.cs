using AutoMapper;
using Dtos.Common.Shared;
using Dtos.Common.v1.Trend;
using Microsoft.Extensions.Logging;
using System.Linq;
using Trend.Application.Interfaces;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Interfaces;

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

            var lastValidSync = await _syncRepo.GetLastValidSync();

            if(lastValidSync is null)
            {
                _logger.LogInformation("No successful sync item found");
                return new List<ArticleDto>();
            }

            var articles = await _articleRepo.GetArticles(lastValidSync.Started, lastValidSync.Finished!.Value, type);

            _logger.LogTrace("Fetched records from database");

            var dtos = _mapper.Map<List<ArticleDto>>(articles);

            _logger.LogTrace("Entities mapped to dtos");

            return dtos;
        }

        public async Task<List<ArticleTypeDto>> GetLatestNews()
        {
            _logger.LogTrace("Fetching latest news");

            var lastValidSync = await _syncRepo.GetLastValidSync();

            if (lastValidSync is null)
            {
                _logger.LogInformation("No successful sync item found");
                return new List<ArticleTypeDto>();
            }

            var articles = await _articleRepo.GetArticles(lastValidSync.Started, lastValidSync.Finished!.Value);

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

            var lastValidSync = await _syncRepo.GetLastValidSync();

            if (lastValidSync is null)
            {
                _logger.LogInformation("No successful sync item found");
                yield break;
            }

            _logger.LogTrace("Mapping to dtos");

            await foreach(var entity in _articleRepo.GetArticlesEnumerable(lastValidSync.Started, lastValidSync.Finished!.Value))
            {
                yield return _mapper.Map<ArticleTypeDto>(entity);
            }
        }

        public async IAsyncEnumerable<ArticleDto> GetLatestNewsEnumerable(ContextType type)
        {
            _logger.LogTrace("Fetching latest news");

            var lastValidSync = await _syncRepo.GetLastValidSync();

            if (lastValidSync is null)
            {
                _logger.LogInformation("No successful sync item found");
                yield break;
            }

            _logger.LogTrace("Mapping to dtos");

            await foreach (var entity in _articleRepo.GetArticlesEnumerable(lastValidSync.Started, lastValidSync.Finished!.Value, type))
            {
                yield return _mapper.Map<ArticleDto>(entity);
            }
        }
    }
}
