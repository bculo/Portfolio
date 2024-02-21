using Crypto.Application.Interfaces.Price.Models;

namespace Crypto.Application.Interfaces.Price
{
    public interface ICryptoPriceService
    {
        Task<PriceResponse> GetPriceInfo(string symbol, CancellationToken ct = default);
        Task<List<PriceResponse>> GetPriceInfo(List<string> symbols, CancellationToken ct = default);
    }
}
