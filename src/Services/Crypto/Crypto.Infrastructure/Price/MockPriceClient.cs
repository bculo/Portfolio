using Crypto.Application.Interfaces.Price;
using Crypto.Application.Interfaces.Price.Models;
using Crypto.Application.Interfaces.Repositories;

namespace Crypto.Infrastructure.Price;

public class MockPriceClient(IUnitOfWork work) : ICryptoPriceService
{
    private const decimal DefaultPriceValue = 0.0m;
    
    public async Task<CryptoAssetPriceResponse> GetPriceInfo(string symbol, CancellationToken ct = default)
    {
        var item = await work.CryptoPriceRepo.GetLastPrice(symbol, ct);
        var response = new CryptoAssetPriceResponse(symbol, "USD", item?.LastPrice ?? DefaultPriceValue);

        Random random = new();
        if (response.Price == DefaultPriceValue)
        {
            response = response with { Price = random.Next(50, 600) };
            return response;
        }
        
        var minValue = response.Price - (response.Price * 0.003m);
        var maxValue = response.Price + (response.Price * 0.003m);
        return response with { Price = (decimal)random.NextDouble() * (maxValue - minValue) + minValue };
    }

    public async Task<List<CryptoAssetPriceResponse>> GetPriceInfo(List<string> symbols, CancellationToken ct = default)
    {
        var finalResult = new List<CryptoAssetPriceResponse>();
        foreach(var symbol in symbols)
        {
            finalResult.Add(await GetPriceInfo(symbol, ct));
        }

        return finalResult;
    }
}