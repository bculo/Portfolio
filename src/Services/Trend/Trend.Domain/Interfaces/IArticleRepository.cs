using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Domain.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        Task DeactivateArticles(List<string> articleIds, CancellationToken token);

        Task<List<Article>> GetActiveArticles(CancellationToken token);
        IAsyncEnumerable<Article> GetActiveArticlesEnumerable(CancellationToken token);

        Task<List<Article>> GetActiveArticles(ContextType type, CancellationToken token);
        IAsyncEnumerable<Article> GetActiveArticlesEnumerable(ContextType type, CancellationToken token);

        Task<List<Article>> GetArticles(DateTime from, DateTime to, CancellationToken token);
        IAsyncEnumerable<Article> GetArticlesEnumerable(DateTime from, DateTime to, CancellationToken token);

        Task<List<Article>> GetArticles(DateTime from, DateTime to, ContextType type, CancellationToken token);
        IAsyncEnumerable<Article> GetArticlesEnumerable(DateTime from, DateTime to, ContextType type, CancellationToken token);
    }
}
