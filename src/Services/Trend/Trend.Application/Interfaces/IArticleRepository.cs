using Trend.Application.Interfaces.Models;
using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces
{
    public interface IArticleRepository : IMongoAuditableRepository<Article>
    {
        Task<List<ArticleDetailResQuery>> GetActive(ContextType type, CancellationToken token = default);
        IAsyncEnumerable<ArticleDetailResQuery> GetActiveEnumerable(ContextType type, CancellationToken token = default);
        Task<PageResQuery<ArticleDetailResQuery>> Filter(FilterArticlesReqQuery search, CancellationToken token);
    }
}
