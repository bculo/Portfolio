using Queryable.Common.Models;

namespace Queryable.Common.Extensions;

public static class PaginatedResultExtensions
{
    public static PaginatedResult<TResult> MapTo<TInput, TResult>(this PaginatedResult<TInput> pageResult,
        Func<TInput, TResult> mapper) where TInput : class where TResult : class
    {
        return new PaginatedResult<TResult>
        {
            PageItems = pageResult.PageItems.Select(mapper).ToList(),
            TotalItemsCount = pageResult.TotalItemsCount
        };
    }
}