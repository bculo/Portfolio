using Microsoft.EntityFrameworkCore;
using Stock.Application.Interfaces.Price;
using Stock.Application.Interfaces.Price.Models;
using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;
using Time.Common;

namespace Stock.Infrastructure.Price;

public class FakePriceClient(IDataSourceProvider dataSourceProvider, IDateTimeProvider provider) : IStockPriceClient
{
    private readonly Random _random = new();

    private readonly IQueryable<StockWithPriceTag> _query =
        dataSourceProvider.GetReadOnlySourceQuery<StockWithPriceTag>();
    
    public async Task<StockPriceInfo?> GetPrice(string symbol, CancellationToken ct = default)
    {
        var item = await _query.SingleOrDefaultAsync(x => x.Symbol == symbol, ct);

        if (item == null)
            return null;
        
        return new StockPriceInfo
        {
            Symbol = symbol,
            FetchedTimestamp = provider.Time,
            Price = item.Price != null ?  _random.Next(50, 600) : GeneratePriceBasedOnPreviousOne(item)
        };
    }
    
    private decimal GeneratePriceBasedOnPreviousOne(StockWithPriceTag item)
    {
        var minValue = item.Price!.Value - (item.Price.Value  * 0.05m);
        var maxValue = item.Price.Value  + (item.Price.Value  * 0.05m);
        return (decimal) _random.NextDouble() * (maxValue - minValue) + minValue;
    }
}