using Crypto.Application.Interfaces.Price.Models;

namespace Crypto.Application.Interfaces.Price
{
    public interface ICryptoPriceService
    {
        Task<CryptoAssetPriceResponse> GetPriceInfo(string symbol, CancellationToken ct = default);
        Task<List<CryptoAssetPriceResponse>> GetPriceInfo(List<string> symbols, CancellationToken ct = default);
    }
}
