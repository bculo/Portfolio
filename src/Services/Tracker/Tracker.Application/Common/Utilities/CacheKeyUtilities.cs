using Tracker.Application.Common.Constants;
using Tracker.Core.Enums;

namespace Tracker.Application.Common.Utilities;

public static class CacheKeyUtilities
{
    private const string CONNECTOR_CHARACTER = "-";

    public static string CombineKey(FinancalAssetType type, string identifier)
    {
        ArgumentException.ThrowIfNullOrEmpty(identifier);
        return $"{DefinePrefix(type)}{CONNECTOR_CHARACTER}{identifier}";
    }

    private static string DefinePrefix(FinancalAssetType type)
    {
        return type switch
        {
            FinancalAssetType.Crypto => CacheConstants.CRYPTO_PREFIX,
            FinancalAssetType.Stock => CacheConstants.STOCK_PREFIX,
            _ => string.Empty
        };
    }
}