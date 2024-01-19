namespace Trend.Application.Interfaces.Models;

public class PageReqQuery
{
    public int Page { get; set; }
    public int Take { get; set; }
    public int Skip => (Page - 1) * Take;
}

public class PageResQuery<T> where T : notnull
{
    public long Count { get; private set; }
    public List<T> Items { get; private set; }

    public PageResQuery(long count, List<T> items)
    {
        Count = count;
        Items = items;
    }
}