using Dtos.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trend.Application.Interfaces;
using Trend.Domain.Entities;
using Trend.Domain.Enums;
using Trend.Domain.Interfaces;

namespace Trend.Application.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ILogger<ArticleService> _logger;
        private readonly IRepository<Article> _articleRepo;
        private readonly IRepository<SyncStatus> _syncRepo;

        public ArticleService(ILogger<ArticleService> logger, 
            IRepository<Article> articleRepo,
            IRepository<SyncStatus> syncRepo)
        {
            _logger = logger;
            _articleRepo = articleRepo;
            _syncRepo = syncRepo;
        }

        public async Task<List<ArticleDto>> GetLatestNews(ArticleType type)
        {
            _logger.LogTrace("Fetching news for type {0}", type);

            var articles = await _articleRepo.GetAll();

            _logger.LogTrace("Fetched records from database");

            //TODO add automapper
            var dtos = articles.Select(i => new ArticleDto
            {
                Id = i.Id.ToString(),
                Title = i.Title,
                Url = i.ArticleUrl
            }).ToList();

            return dtos;
        }
    }
}
