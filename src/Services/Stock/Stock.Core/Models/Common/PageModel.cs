using Stock.Core.Models.Base;

namespace Stock.Core.Models.Common;

public class PageModel<T>(long totalCount, int page, List<T> items) : IReadModel
{
    public long TotalCount { get; init; } = totalCount;
    public long FetchCount { get; init; } = items.Count;
    public int Page { get; init; } = page;
    public List<T> Items { get; init; } = items;
}