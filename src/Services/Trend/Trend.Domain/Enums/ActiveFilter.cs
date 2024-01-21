using Trend.Domain.Exceptions;

namespace Trend.Domain.Enums;

public record ActiveFilter(int Id, string DisplayValue) 
    : Enumeration<ActiveFilter>(Id, DisplayValue)
{
    public static readonly ActiveFilter Active = new (0, "Active items");
    public static readonly ActiveFilter Inactive = new (1, "Inactive items");
    public static readonly ActiveFilter All = new (999, "Show all");
    
    public static implicit operator int(ActiveFilter context) => context.Id;
    public static implicit operator ActiveFilter(int value) => Create(value);
}