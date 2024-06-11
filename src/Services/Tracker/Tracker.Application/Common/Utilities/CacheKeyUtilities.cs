using Tracker.Application.Common.Constants;
using Tracker.Core.Enums;

namespace Tracker.Application.Common.Utilities;

public static class CacheKeyUtilities
{
    private const string CONNECTOR_CHARACTER = "-";

    public static string CombineKey(FinancialAssetType type, string identifier)
    {
        ArgumentException.ThrowIfNullOrEmpty(identifier);
        return $"{DefinePrefix(type)}{CONNECTOR_CHARACTER}{identifier}";
    }

    private static string DefinePrefix(FinancialAssetType type)
    {
        return type switch
        {
            FinancialAssetType.Crypto => CacheConstants.CRYPTO_PREFIX,
            FinancialAssetType.Stock => CacheConstants.STOCK_PREFIX,
            _ => string.Empty
        };
    }
}