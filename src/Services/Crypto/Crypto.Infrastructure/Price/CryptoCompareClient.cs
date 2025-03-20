using Crypto.Application.Common.Constants;
using Crypto.Application.Common.Options;
using Crypto.Application.Interfaces.Price;
using Crypto.Application.Interfaces.Price.Models;
using Crypto.Core.Exceptions;
using Http.Common.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Crypto.Infrastructure.Price
{
    public class CryptoCompareClient(
        IHttpClientFactory httpClientFactory, 
        IOptions<CryptoPriceApiOptions> options)
        : ICryptoPriceService
    {
        private readonly CryptoPriceApiOptions _options = options.Value;

        public async Task<CryptoAssetPriceResponse> GetPriceInfo(string symbol, CancellationToken ct = default)
        {
            var client = httpClientFactory.CreateClient(ApiClient.CryptoPrice);
            var response = await client.GetAsync($"price?fsym={symbol.ToUpper()}&tsyms={_options.Currency}", ct);
            var content = await response.ExtractContentFromResponse();

            if(IsBadRequest(content))
                throw new CryptoCoreException("Problem occurred on price fetch");

            var finalContent = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(content);
            if (finalContent is null)
                throw new CryptoCoreException("Problem occurred on price fetch");
            
            return new CryptoAssetPriceResponse(symbol.ToUpper(), _options.Currency, finalContent[_options.Currency]);
        }

        public async Task<List<CryptoAssetPriceResponse>> GetPriceInfo(List<string> symbols, CancellationToken ct = default)
        {
            var client = httpClientFactory.CreateClient(ApiClient.CryptoPrice);
            
            var response = await client.GetAsync(
                $"pricemulti?fsyms={ConvertSymbolsArrayToString(symbols)}&tsyms={_options.Currency}", 
                ct);
            
            var content = await response.ExtractContentFromResponse();
            
            if (IsBadRequest(content))
                throw new CryptoCoreException("Couldn't fetch item price");

            var finalContent = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, decimal>>>(content);
            if (finalContent is null)
                throw new CryptoCoreException("Couldn't fetch item price");
            
            List<CryptoAssetPriceResponse> finalResult = [];
            finalResult.AddRange(finalContent.Select(item =>
                new CryptoAssetPriceResponse(item.Key, _options.Currency, item.Value[_options.Currency])));

            return finalResult;
        }

        /// <summary>
        ///  Crypto compare returns status code 200 even if provided symbol incorrect
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private bool IsBadRequest(string? content)
        {
            if(content is null)
                return true;
            return content.Contains("Response") && content.Contains("Error") && content.Contains("Message");
        }

        private string ConvertSymbolsArrayToString(IEnumerable<string> symbols)
        {
            return string.Join(",", symbols.Select(i => i.ToUpper()));
        }
    }
}
