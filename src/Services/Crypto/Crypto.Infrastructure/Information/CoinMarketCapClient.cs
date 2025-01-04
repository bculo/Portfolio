using Crypto.Application.Common.Constants;
using Crypto.Application.Interfaces.Information;
using Crypto.Application.Interfaces.Information.Models;
using Crypto.Core.Exceptions;
using Crypto.Infrastructure.Information.Models;
using Http.Common.Extensions;
using Microsoft.Extensions.Logging;

namespace Crypto.Infrastructure.Information
{
    public class CoinMarketCapClient(
        IHttpClientFactory clientFactory,
        ILogger<CoinMarketCapClient> logger)
        : ICryptoInfoService
    {
        private readonly ILogger<CoinMarketCapClient> _logger = logger;

        public async Task<CryptoInformation> GetInformation(string symbol,  CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(symbol);
            
            var client = clientFactory.CreateClient(ApiClient.CryptoInfo);
            
            var response = await client.GetAsync($"info?symbol={symbol}", ct);
            
            var responseInstance =  await response.ExtractContentFromResponse<CoinMarketCapRootResponseDto>();

            return MakeClientResponseFlat(responseInstance, symbol);
        }

        private CryptoInformation MakeClientResponseFlat(CoinMarketCapRootResponseDto responseInstance, string symbol)
        {
            var cryptoData = responseInstance.Data.Values.FirstOrDefault();
            if (cryptoData is null || cryptoData.Count == 0)
            {
                throw new CryptoCoreException($"Data for given symbol {symbol} not found");
            }

            var info = cryptoData.FirstOrDefault(i => string.Equals(i.Symbol, symbol, StringComparison.CurrentCultureIgnoreCase));
            if (info is null)
            {
                throw new CryptoCoreException($"Data for given symbol {symbol} not found");
            }

            var website = info.Urls.TryGetValue("website", out var urls) ? urls.FirstOrDefault() : null;
            var source = info.Urls.TryGetValue("source_code", out var sources) ? sources!.FirstOrDefault() : null;

            return new CryptoInformation
            {
                Logo = info.Logo,
                Name = info.Name,
                Symbol = info.Symbol,
                Description = info.Description,
                Website = website,
                SourceCode = source
            };
        }
    }
}
