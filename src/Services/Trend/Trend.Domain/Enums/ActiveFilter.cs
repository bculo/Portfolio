using Trend.Domain.Exceptions;

namespace Trend.Domain.Enums;

public record ActiveFilter(int Id, string Name, bool? Value)
{
    public static ActiveFilter Active { get; } = new (0, "Active items", true);
    public static ActiveFilter Inactive { get; } = new (1, "Inactive items", false);
    public static ActiveFilter All { get; } = new (999, "Show all", default);

    public static implicit operator int(ActiveFilter context) => context.Id;
        
    public static implicit operator ActiveFilter(int id) => Create(id);
        
    private static ActiveFilter Create(int id) =>
        id switch
        {
            0 => Active,
            1 => Inactive,
            999 => All,
            _ => throw new TrendAppCoreException($"Active filter is not supported: {id}")
        };

    public override string ToString()
    {
        return Name;
    }
        
    public bool IsRelevantForFilter()
    {
        return All.Id != Id;
    }

    public static bool IsValidItem(int id)
    {
        return GetPossibleOptions().Any(i => i.Id == id);
    }

    public static IEnumerable<ActiveFilter> GetPossibleOptions()
    {
        yield return Active;
        yield return Inactive;
        yield return All;
    }
}