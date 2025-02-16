using System.Linq.Expressions;

namespace Queryable.Common.Models;

public enum SortDirection
{
    Ascending = 0,
    Descending = 1
}

public record StringSort(string PropertyName, SortDirection Direction);
public record Sort<T>(T SortOption, SortDirection Direction) where T : struct, Enum;
public class SortDefinition<TOption, TModel>(TOption defaultOption) : Dictionary<TOption, Expression<Func<TModel, object?>>>
    where TOption : struct, Enum where TModel : class
{
    public TOption DefaultOption { get; } = defaultOption;
}