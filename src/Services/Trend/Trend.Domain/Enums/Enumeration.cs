using System.Reflection;
using Trend.Domain.Exceptions;

namespace Trend.Domain.Enums;

public abstract record Enumeration<T>(int Value, string DisplayValue) where T : Enumeration<T>
{
    private static readonly Lazy<Dictionary<int, T>> AllItems;

    static Enumeration()
    {
        AllItems = new Lazy<Dictionary<int, T>>(() =>
        {
            return typeof(T)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(x => x.FieldType == typeof(T))
                .Select(x => x.GetValue(null))
                .Cast<T>()
                .ToDictionary(x => x.Value, x => x);
        });
    }

    public override string ToString()
    {
        return DisplayValue;
    }

    public static IEnumerable<T> GetAll()
    {
        return AllItems.Value.Values;
    }
    
    public static T? TryFromValue(int value)
    {
        if (IsValidItem(value))
        {
            return AllItems.Value[value];
        }

        return null;
    }

    public static T FromValue(int value)
    {
        if (IsValidItem(value))
        {
            return AllItems.Value[value];
        }
        
        throw new TrendAppCoreException($"ID with value {value} is not part of the enum {typeof(T).Name}");
    }

    public static bool IsValidItem(int value)
    {
        return AllItems.Value.ContainsKey(value);
    }
    
    protected static T Create(int value)
    {
        if (IsValidItem(value))
        {
            return AllItems.Value[value];
        }

        throw new TrendAppCoreException($"ID with value {value} is not part of the enum {typeof(T).Name}");
    }
}