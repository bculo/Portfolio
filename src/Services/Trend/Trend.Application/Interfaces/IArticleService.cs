using Dtos.Common;
using LanguageExt;
using Trend.Application.Interfaces.Models.Dtos;
using Trend.Domain.Enums;
using Trend.Domain.Errors;

namespace Trend.Application.Interfaces
{
    public interface IArticleService
    {
        Task<List<ArticleResDto>> GetLatestNewsByContextType(ContextType type, CancellationToken tcs = default);
        Task<PageResponseDto<ArticleResDto>> GetLatestNewsPage(ArticleFetchPageReqDto page, CancellationToken tcs = default);
        Task<List<ArticleResDto>> GetLatestNews(CancellationToken tcs = default);
        Task<Either<CoreError, Unit>> Deactivate(string articleId, CancellationToken tcs = default);
        Task<Either<CoreError, Unit>> Activate(string articleId, CancellationToken tcs = default);
    }
}
