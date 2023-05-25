namespace Tracker.Application.Utilities;

public static class CacheKeyUtilities
{
    private const string CONNECTOR_CHARACTER = "-";
    
    public static string CombineKey(string prefix, string identifier)
    {
        ArgumentException.ThrowIfNullOrEmpty(prefix);
        ArgumentException.ThrowIfNullOrEmpty(identifier);
        return $"{prefix}{CONNECTOR_CHARACTER}{identifier}";
    }
}