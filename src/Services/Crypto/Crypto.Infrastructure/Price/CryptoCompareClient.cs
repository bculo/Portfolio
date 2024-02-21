using Crypto.Application.Common.Constants;
using Crypto.Application.Common.Options;
using Crypto.Application.Interfaces.Price;
using Crypto.Application.Interfaces.Price.Models;
using Crypto.Core.Exceptions;
using Http.Common.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Crypto.Infrastructure.Clients
{
    public class CryptoCompareClient : ICryptoPriceService
    {
        private readonly CryptoPriceApiOptions _options;
        private readonly IHttpClientFactory _httpClientFactory;

        public CryptoCompareClient(IHttpClientFactory httpClientFactory, IOptions<CryptoPriceApiOptions> options)
        {
            _options = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<PriceResponse> GetPriceInfo(string symbol, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(symbol);

            var client = _httpClientFactory.CreateClient(ApiClient.CryptoPrice);
            var response = await client.GetAsync($"price?fsym={symbol.ToUpper()}&tsyms={_options.Currency}", ct);
            var content = await response.HandleResponse();

            if(IsBadRequest(content)) //Crypto compare returns status code 200 even if provided symbol incorrect
            {
                throw new CryptoCoreException("Problem occurred on price fetch");
            }

            var finalContent = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(content);
            if (finalContent is null)
            {
                throw new CryptoCoreException("Problem occurred on price fetch");
            }
            
            return new PriceResponse
            {
                Currency = _options.Currency,
                Price = finalContent![_options.Currency],
                Symbol = symbol.ToUpper()
            };
        }

        public async Task<List<PriceResponse>> GetPriceInfo(List<string> symbols, CancellationToken ct = default)
        {
            ArgumentNullException.ThrowIfNull(symbols);

            var client = _httpClientFactory.CreateClient(ApiClient.CryptoPrice);
            var response = await client.GetAsync(
                $"pricemulti?fsyms={ConvertSymbolsArrayToString(symbols)}&tsyms={_options.Currency}", 
                ct);
            var content = await response.HandleResponse();
            
            if (IsBadRequest(content)) //Crypto compare returns status code 200 even if provided symbol incorrect
            {
                throw new CryptoCoreException("Problem occurred on price fetch");
            }

            var finalContent = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, decimal>>>(content);
            if (finalContent is null)
            {
                throw new CryptoCoreException("Problem occurred on price fetch");
                
            }
            
            var finalResult = new List<PriceResponse>();
            foreach(var item in finalContent)
            {
                finalResult.Add(new PriceResponse
                {
                    Currency = _options.Currency,
                    Symbol = item.Key,
                    Price = item.Value[_options.Currency]
                });
            }

            return finalResult;
        }

        private bool IsBadRequest(string? content)
        {
            if(content is null)
            {
                return true;
            }

            return content.Contains("Response") && content.Contains("Error") && content.Contains("Message");
        }

        private string ConvertSymbolsArrayToString(IEnumerable<string> symbols)
        {
            return string.Join(",", symbols.Select(i => i.ToUpper()));
        }
    }
}
