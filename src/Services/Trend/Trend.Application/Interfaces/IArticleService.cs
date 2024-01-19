using Dtos.Common;
using LanguageExt;
using Trend.Application.Interfaces.Models;
using Trend.Domain.Enums;
using Trend.Domain.Errors;

namespace Trend.Application.Interfaces
{
    public interface IArticleService
    {
        Task<List<ArticleResDto>> GetLatestNewsByContextType(ContextType type, CancellationToken tcs = default);
        Task<PageResponseDto<ArticleResDto>> GetLatestNewsPage(FetchArticlePageReqDto page, CancellationToken tcs = default);
        Task<List<ArticleResDto>> GetLatestNews(CancellationToken tcs = default);
        Task<Either<CoreError, Unit>> Deactivate(DeactivateArticleReqDto articleId, CancellationToken tcs = default);
        Task<Either<CoreError, Unit>> Activate(ActivateArticleReqDto req, CancellationToken tcs = default);
    }
}
