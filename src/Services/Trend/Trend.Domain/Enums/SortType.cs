using Trend.Domain.Exceptions;

namespace Trend.Domain.Enums;

public record SortType(int Id, string Name)
{
    public static SortType Asc { get; } = new (0, "Ascending");
    public static SortType Desc { get; } = new (1, "Descending");
    
    public static implicit operator int(SortType context) => context.Id;
        
    public static implicit operator SortType(int id) => Create(id);
        
    private static SortType Create(int id) =>
        id switch
        {
            0 => Asc,
            1 => Desc,
            _ => throw new TrendAppCoreException($"Sort filter is not supported: {id}")
        };
    
    public override string ToString()
    {
        return Name;
    }
    
    public static bool IsValidItem(int id)
    {
        return GetPossibleOptions().Any(i => i.Id == id);
    }
    
    public static IEnumerable<SortType> GetPossibleOptions()
    {
        yield return Asc;
        yield return Desc;
    }
}