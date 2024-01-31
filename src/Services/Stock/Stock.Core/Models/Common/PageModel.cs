using Stock.Core.Models.Base;

namespace Stock.Core.Models.Common;

public class PageModel<T> : IReadModel
{
    public long TotalCount { get; init; }
    public long FetchCount { get; init; }
    public int Page { get; init; }
    public List<T> Items { get; init; }
    
    public PageModel(long totalCount, int page, List<T> items)
    {
        TotalCount = totalCount;
        FetchCount = items.Count;
        Items = items;
        Page = page;
    }
}