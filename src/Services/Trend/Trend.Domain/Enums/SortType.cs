using Trend.Domain.Exceptions;

namespace Trend.Domain.Enums;

public record SortType(int Value, string DisplayValue) : Enumeration<SortType>(Value, DisplayValue)
{
    public static readonly SortType Asc = new (0, "Ascending");
    public static readonly SortType Desc = new (1, "Descending");
    
    public static implicit operator int(SortType context) => context.Value;
    public static implicit operator SortType(int value) => Create(value);
}