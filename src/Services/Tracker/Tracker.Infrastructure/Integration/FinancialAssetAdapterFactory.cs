using Tracker.Application.Interfaces.Integration;
using Tracker.Core.Enums;

namespace Tracker.Infrastructure.Integration;

internal class FinancialAssetAdapterFactory(IServiceProvider provider) : IFinancialAssetAdapterFactory
{
    public IFinancialAssetAdapter GetAdapter(FinancialAssetType type)
    {
        return type switch
        {
            FinancialAssetType.Crypto => provider.GetService(typeof(CryptoAdapter)) as IFinancialAssetAdapter,
            FinancialAssetType.Stock => provider.GetService(typeof(StockAdapter)) as IFinancialAssetAdapter,
            _ => throw new NotSupportedException($"Financial asset client for type {type} not supported")
        };
    }
}

