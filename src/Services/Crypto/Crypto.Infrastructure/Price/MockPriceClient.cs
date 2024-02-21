using Crypto.Application.Interfaces.Price;
using Crypto.Application.Interfaces.Price.Models;
using Crypto.Application.Interfaces.Repositories;
using Time.Abstract.Contracts;

namespace Crypto.Infrastructure.Price;

public class MockPriceClient : ICryptoPriceService
{
    private readonly IUnitOfWork _work;

    public MockPriceClient(IUnitOfWork work)
    {
        _work = work;
    }

    public async Task<PriceResponse> GetPriceInfo(string symbol, CancellationToken ct = default)
    {
        var item = await _work.CryptoPriceRepo.GetLastPrice(symbol, ct);

        var price = item?.Price ?? default(decimal);
        
        var response = new PriceResponse
        {
            Symbol = symbol,
        };

        var random = new Random();
        
        if (price == default)
        {
            response.Price = random.Next(50, 600);
            return response;
        }
        
        var minValue = price - (price * 0.05m);
        var maxValue = price + (price * 0.05m);
        response.Price = (decimal)random.NextDouble() * (maxValue - minValue) + minValue;
        
        return response;
    }

    public async Task<List<PriceResponse>> GetPriceInfo(List<string> symbols, CancellationToken ct = default)
    {
        var finalResult = new List<PriceResponse>();
        foreach(var symbol in symbols)
        {
            finalResult.Add(await GetPriceInfo(symbol, ct));
        }

        return finalResult;
    }
}