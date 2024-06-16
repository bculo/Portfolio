using Api.GeneratedCode;
using Tracker.Application.Interfaces.Integration;
using Tracker.Application.Interfaces.Integration.Models;

namespace Tracker.Infrastructure.Integration;

public class CryptoAdapter(ICryptoApi cryptoApi) : IFinancialAssetAdapter
{
    public async Task<FinancialAssetDto?> FetchAsset(string symbol, CancellationToken ct = default)
    {
        var result = await cryptoApi.Single(symbol);

        if (!result.IsSuccessStatusCode || result.Content is null)
        {
            return default;
        }

        return new FinancialAssetDto(result.Content.Symbol, result.Content.Price, result.Content.Name);
    }
}