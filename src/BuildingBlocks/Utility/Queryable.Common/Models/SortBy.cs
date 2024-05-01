namespace Queryable.Common.Models;

public enum SortDirection
{
    Ascending = 0,
    Descending = 1
}

public record SortBy(string PropertyName, SortDirection Direction);