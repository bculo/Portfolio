using Stock.Application.Interfaces.Price;
using Stock.Application.Interfaces.Price.Models;
using Stock.Application.Interfaces.Repositories;
using Time.Abstract.Contracts;

namespace Stock.Infrastructure.Price;

public class FakePriceClient : IStockPriceClient
{
    private readonly IUnitOfWork _work;
    private readonly IDateTimeProvider _provider;

    public FakePriceClient(IUnitOfWork work, IDateTimeProvider provider)
    {
        _work = work;
        _provider = provider;
    }
    
    public async Task<StockPriceInfo?> GetPrice(string symbol, CancellationToken ct = default)
    {
        var result = await _work.StockWithPriceTag.First(x => x.Symbol == symbol, ct: ct);
        
        var response = new StockPriceInfo
        {
            Symbol = symbol,
            FetchedTimestamp = _provider.Utc
        };

        var random = new Random();
        
        if (result is null || result.Price == default)
        {
            response.Price = random.Next(50, 600);
            return response;
        }
        
        var minValue = result.Price - (result.Price * 0.05m);
        var maxValue = result.Price + (result.Price * 0.05m);
        response.Price = (decimal)random.NextDouble() * (maxValue - minValue) + minValue;

        return response;
    }
}