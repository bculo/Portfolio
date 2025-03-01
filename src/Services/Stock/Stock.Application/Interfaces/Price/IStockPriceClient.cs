using Stock.Application.Interfaces.Price.Models;

namespace Stock.Application.Interfaces.Price
{
    public interface IStockPriceClient
    {
        Task<StockPriceInfo?> GetPrice(string symbol, CancellationToken ct = default);
    }
}
