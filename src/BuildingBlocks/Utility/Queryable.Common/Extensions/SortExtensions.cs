using System.Linq.Expressions;
using Queryable.Common.Models;

namespace Queryable.Common.Extensions;

public static class StringSortExtensions
{
    public static string ToSortExpression(this StringSort stringSort)
    {
        if (stringSort.Direction == SortDirection.Ascending)
        {
            return stringSort.PropertyName;
        }
        
        return $"{stringSort.PropertyName} desc";
    }
}

public static class SortByExtensions
{
    public static IOrderedQueryable<TModel> SortBy<TModel, TOption>(this IQueryable<TModel> query,
        Sort<TOption> selectedSort, SortDefinition<TOption, TModel> sortOptions)
        where TModel : class where TOption : struct, Enum
    {
        var sortExpression = GetSortExpression(selectedSort, sortOptions);
        
        return selectedSort.Direction == SortDirection.Ascending
            ? query.OrderBy(sortExpression)
            : query.OrderByDescending(sortExpression);
    }
    
    public static IOrderedQueryable<TModel> ThenSortBy<TModel, TOption>(this IOrderedQueryable<TModel> query,
        Sort<TOption> selectedSort, SortDefinition<TOption, TModel> sortOptions)
        where TModel : class where TOption : struct, Enum
    {
        var sortExpression = GetSortExpression(selectedSort, sortOptions);
        
        return selectedSort.Direction == SortDirection.Ascending
            ? query.ThenBy(sortExpression)
            : query.ThenByDescending(sortExpression);
    }

    private static Expression<Func<TModel, object?>> GetSortExpression<TModel, TOption>(Sort<TOption> selectedSort,
        SortDefinition<TOption, TModel> sortOptions)
        where TModel : class where TOption : struct, Enum
    {
        var selectedSortExist = sortOptions.TryGetValue(selectedSort.SortOption, out var selectedSortExpression);
        if (selectedSortExist)
            return selectedSortExpression;
        
        if (sortOptions.TryGetValue(selectedSort.SortOption, out var defaultSortExpression))
            return defaultSortExpression;

        throw new ArgumentException("Selected sort option not available");
    }
}