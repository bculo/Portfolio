namespace Crypto.Application.Interfaces.Repositories.Models;

public class PageModel<T> where T : class
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