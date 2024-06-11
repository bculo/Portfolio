using Tracker.Application.Interfaces.Integration.Models;

namespace Tracker.Application.Interfaces.Integration;

public interface IFinancialAssetAdapter
{
    Task<FinancialAssetDto> FetchAsset(string symbol, CancellationToken ct = default);
}

