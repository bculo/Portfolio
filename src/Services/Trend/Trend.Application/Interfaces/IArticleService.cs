using Dtos.Common;
using Trend.Application.Interfaces.Models.Dtos;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces
{
    public interface IArticleService
    {
        Task<List<ArticleResDto>> GetLatestNewsByContextType(ContextType type, CancellationToken tcs = default);
        Task<PageResponseDto<ArticleResDto>> GetLatestNewsPage(ArticleFetchPageReqDto page, CancellationToken tcs = default);
        Task<List<ArticleResDto>> GetLatestNews(CancellationToken tcs = default);
        Task Deactivate(string articleId, CancellationToken tcs = default);
        Task Activate(string articleId, CancellationToken tcs = default);
    }
}
