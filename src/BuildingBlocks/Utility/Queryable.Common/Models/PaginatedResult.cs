namespace Queryable.Common.Models;

public class PaginatedResult<T> where T : class
{
    public int TotalItemsCount { get; set; }
    public List<T> PageItems { get; set; } = [];
}