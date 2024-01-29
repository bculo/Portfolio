using Stock.Core.Models.Base;

namespace Stock.Core.Models.Common;

public class PageReadModel<T>
{
    public long TotalCount { get; init; }
    public long FetchCount { get; init; }
    public int Page { get; init; }
    public List<T> Items { get; init; }
    
    public PageReadModel(long totalCount, int page, List<T> items)
    {
        TotalCount = totalCount;
        FetchCount = items.Count;
        Items = items;
        Page = page;
    }
}