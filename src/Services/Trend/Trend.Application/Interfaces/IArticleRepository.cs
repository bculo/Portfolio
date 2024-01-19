using Trend.Application.Interfaces.Models;
using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces
{
    public interface IArticleRepository : IMongoAuditableRepository<Article>
    {
        Task<List<ArticleDetailResQuery>> GetActiveArticles(ContextType type, CancellationToken token = default);
        IAsyncEnumerable<ArticleDetailResQuery> GetActiveArticlesEnumerable(ContextType type, CancellationToken token = default);
    }
}
