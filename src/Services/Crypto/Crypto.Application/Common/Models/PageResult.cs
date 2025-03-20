namespace Crypto.Application.Common.Models;

public class PageResult<T>(long totalCount, int page, List<T> items) where T : class
{
    public long TotalCount { get; init; } = totalCount;
    public long FetchCount { get; init; } = items.Count;
    public int Page { get; init; } = page;
    public List<T> Items { get; init; } = items;
}