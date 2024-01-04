using Trend.Application.Interfaces.Models.Repositories;
using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces
{
    public interface IArticleRepository : IMongoAuditableRepository<Article>
    {
        Task<List<ArticleDetailResQuery>> GetActiveArticles(ContextType type, CancellationToken token);
        IAsyncEnumerable<ArticleDetailResQuery> GetActiveArticlesEnumerable(ContextType type, CancellationToken token);
    }
}
