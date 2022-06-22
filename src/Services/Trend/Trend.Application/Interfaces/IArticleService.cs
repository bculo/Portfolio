using Dtos.Common.Shared;
using Dtos.Common.v1.Trend;
using Dtos.Common.v1.Trend.Article;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces
{
    public interface IArticleService
    {
        Task<List<ArticleDto>> GetLatestNews(ContextType type);
        Task<PageResponseDto<ArticleDto>> GetLatestNewsPage(FetchArticleTypePageDto page);

        Task<List<ArticleTypeDto>> GetLatestNews();
        Task<PageResponseDto<ArticleTypeDto>> GetLatestNewsPage(FetchLatestNewsPageDto page);

        IAsyncEnumerable<ArticleTypeDto> GetAllEnumerable();
        IAsyncEnumerable<ArticleTypeDto> GetLatestNewsEnumerable();
        IAsyncEnumerable<ArticleDto> GetLatestNewsEnumerable(ContextType type);
    }
}
