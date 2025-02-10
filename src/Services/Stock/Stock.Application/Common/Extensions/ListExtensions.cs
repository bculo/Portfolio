namespace Stock.Application.Common.Extensions;

public static class ListExtensions
{
    public static List<T> RemoveNulls<T>(this List<T?> list) where T : class
    {
        return list.Where(item => item != null).ToList()!;
    } 
    
    public static List<T> RemoveNulls<T>(this T?[] list) where T : class
    {
        return list.Where(item => item != null).ToList()!;
    } 
}