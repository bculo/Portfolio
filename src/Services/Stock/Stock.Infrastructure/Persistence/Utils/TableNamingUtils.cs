namespace Stock.Infrastructure.Persistence.Utils;

public static class TableNamingUtils
{
    public static string ToTableName<T>()
    {
        return typeof(T).Name.ToLower();
    }
}