using Dtos.Common.Shared;
using Dtos.Common.v1.Trend;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces
{
    public interface IArticleService
    {
        Task<List<ArticleDto>> GetLatestNews(ContextType type);
        Task<List<ArticleTypeDto>> GetLatestNews();
        IAsyncEnumerable<ArticleTypeDto> GetAllEnumerable();
        IAsyncEnumerable<ArticleTypeDto> GetLatestNewsEnumerable();
        IAsyncEnumerable<ArticleDto> GetLatestNewsEnumerable(ContextType type);
    }
}
