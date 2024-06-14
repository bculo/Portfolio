using Stock.gRPC;
using Tracker.Application.Interfaces.Integration;
using Tracker.Application.Interfaces.Integration.Models;

namespace Tracker.Infrastructure.Integration;

public class StockAdapter(Stock.gRPC.Stock.StockClient grpcClient) : IFinancialAssetAdapter
{
    public async Task<FinancialAssetDto?> FetchAsset(string symbol, CancellationToken ct = default)
    {
        var response = await grpcClient.GetBySymbolAsync(new GetBySymbolRequest { Symbol = symbol });

        if (response?.Price == null)
        {
            return default;
        }
        
        return new FinancialAssetDto(response.Symbol, response.Price.Value, response.Symbol);
    }
}