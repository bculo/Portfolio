using Queryable.Common.Models;

namespace Queryable.Common.Extensions;

public static class SortByExtensions
{
    public static string ToSortExpression(this SortBy sortBy)
    {
        if (sortBy.Direction == SortDirection.Ascending)
        {
            return sortBy.PropertyName;
        }
        
        return $"{sortBy} desc";
    }
}