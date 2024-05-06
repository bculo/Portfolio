namespace Crypto.Infrastructure.Persistence.Extensions;

public static class LikeExtensions
{
    public static string ToContainsPattern(this string query)
    {
        return $"%{query}%";
    }
}