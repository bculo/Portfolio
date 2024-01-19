using Trend.Application.Interfaces.Models;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces;

public interface IArticleServiceEnumerable
{
    IAsyncEnumerable<ArticleResDto> GetAllEnumerable(CancellationToken tcs = default);
    IAsyncEnumerable<ArticleResDto> GetLatestEnumerable(CancellationToken tcs = default);
    IAsyncEnumerable<ArticleResDto> GetLatestByContextEnumerable(ContextType type, CancellationToken tcs = default);
}