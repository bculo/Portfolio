using Crypto.Application.Interfaces.Price;
using Crypto.Application.Interfaces.Price.Models;
using Crypto.Application.Interfaces.Repositories;

namespace Crypto.Infrastructure.Price;

public class MockPriceClient(IUnitOfWork work) : ICryptoPriceService
{
    public async Task<PriceResponse> GetPriceInfo(string symbol, CancellationToken ct = default)
    {
        var item = await work.CryptoPriceRepo.GetLastPrice(symbol, ct);

        var price = item?.LastPrice ?? default(decimal);
        
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
        
        var minValue = price - (price * 0.003m);
        var maxValue = price + (price * 0.003m);
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