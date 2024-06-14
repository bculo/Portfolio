using Tracker.Application.Interfaces.Integration;
using Tracker.Application.Interfaces.Integration.Models;

namespace Tracker.Infrastructure.Integration;

public class CryptoAdapter : IFinancialAssetAdapter
{
    public Task<FinancialAssetDto?> FetchAsset(string symbol, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}