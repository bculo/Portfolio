namespace Trend.Application.Extensions;

public static class PrimitiveTypeExtensions
{
    public static IEnumerable<T> ToEnumerable<T>(this T instance)
    {
        return new List<T>() { instance };
    } 
}