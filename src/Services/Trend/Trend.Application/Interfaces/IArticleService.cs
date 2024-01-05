using Dtos.Common;
using Trend.Application.Interfaces.Models.Dtos;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces
{
    public interface IArticleService
    {
        Task<List<ArticleResDto>> GetLatestNewsByContextType(ContextType type, CancellationToken tcs);
        Task<PageResponseDto<ArticleResDto>> GetLatestNewsPage(ArticleFetchPageReqDto page, CancellationToken tcs);
        Task<List<ArticleResDto>> GetLatestNews(CancellationToken tcs);
        Task Deactivate(string articleId, CancellationToken tcs);
        Task Activate(string articleId, CancellationToken tcs);
    }
}
