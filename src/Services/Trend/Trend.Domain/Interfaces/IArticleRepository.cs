using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Domain.Interfaces
{
    public interface IArticleRepository : IMongoAuditableRepository<Article>
    {
        Task<List<Article>> GetActiveArticles(ContextType type, CancellationToken token);
        IAsyncEnumerable<Article> GetActiveArticlesEnumerable(ContextType type, CancellationToken token);
    }
}
