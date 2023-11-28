using Dtos.Common.Shared;
using Dtos.Common.v1.Trend;
using Dtos.Common.v1.Trend.Article;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces
{
    public interface IArticleService
    {
        Task<List<ArticleDto>> GetLatestNews(ContextType type, CancellationToken tcs);
        Task<PageResponseDto<ArticleDto>> GetLatestNewsPage(FetchArticleTypePageDto page, CancellationToken tcs);
        Task<List<ArticleTypeDto>> GetLatestNews(CancellationToken tcs);
        Task<PageResponseDto<ArticleTypeDto>> GetLatestNewsPage(FetchLatestNewsPageDto page, CancellationToken tcs);
        IAsyncEnumerable<ArticleTypeDto> GetAllEnumerable(CancellationToken tcs);
        IAsyncEnumerable<ArticleTypeDto> GetLatestNewsEnumerable(CancellationToken tcs);
        IAsyncEnumerable<ArticleDto> GetLatestNewsEnumerable(ContextType type, CancellationToken tcs);
    }
}
