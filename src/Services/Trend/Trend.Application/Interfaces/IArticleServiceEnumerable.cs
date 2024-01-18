using Trend.Application.Interfaces.Models.Dtos;
using Trend.Domain.Enums;

namespace Trend.Application.Interfaces;

public interface IArticleServiceEnumerable : IArticleService
{
    IAsyncEnumerable<ArticleResDto> GetAllEnumerable(CancellationToken tcs = default);
    IAsyncEnumerable<ArticleResDto> GetLatestNewsEnumerable(CancellationToken tcs = default);
    IAsyncEnumerable<ArticleResDto> GetLatestNewsEnumerable(ContextType type, CancellationToken tcs = default);
}