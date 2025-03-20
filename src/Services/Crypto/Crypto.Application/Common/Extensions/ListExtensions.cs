namespace Crypto.Application.Common.Extensions;

public static class ListExtensions
{
    public static List<TOutput> MapTo<TInput, TOutput>(this IEnumerable<TInput> items, Func<TInput, TOutput> mapper)
        => items.Select(mapper).ToList();
    
}