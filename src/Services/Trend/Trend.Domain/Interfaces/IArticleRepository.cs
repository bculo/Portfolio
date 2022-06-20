using Trend.Domain.Entities;
using Trend.Domain.Enums;

namespace Trend.Domain.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        Task DeactivateArticles(List<string> articleIds);

        Task<List<Article>> GetActiveArticles();
        IAsyncEnumerable<Article> GetActiveArticlesEnumerable();

        Task<List<Article>> GetActiveArticles(ContextType type);
        IAsyncEnumerable<Article> GetActiveArticlesEnumerable(ContextType type);

        Task<List<Article>> GetArticles(DateTime from, DateTime to);
        IAsyncEnumerable<Article> GetArticlesEnumerable(DateTime from, DateTime to);

        Task<List<Article>> GetArticles(DateTime from, DateTime to, ContextType type);
        IAsyncEnumerable<Article> GetArticlesEnumerable(DateTime from, DateTime to, ContextType type);
    }
}
