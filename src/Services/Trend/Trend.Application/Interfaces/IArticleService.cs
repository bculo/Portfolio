using Dtos.Common.v1.Trend;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces
{
    public interface IArticleService
    {
        Task<List<ArticleDto>> GetLatestNews(ArticleType type);
        Task<List<ArticleTypeDto>> GetLatestNews();
    }
}
