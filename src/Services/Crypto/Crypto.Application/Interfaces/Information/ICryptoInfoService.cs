using Crypto.Application.Interfaces.Information.Models;

namespace Crypto.Application.Interfaces.Information
{
    public interface ICryptoInfoService
    {
        Task<CryptoInformation> GetInformation(string symbol, CancellationToken ct = default);
    }
}
