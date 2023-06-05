using Crypto.Application.Interfaces.Services;
using Crypto.Application.Models.Price;
using Crypto.Application.Options;
using Crypto.Infrastracture.Constants;
using Http.Common.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Crypto.Infrastracture.Clients
{
    public class CryptoCompareClient : ICryptoPriceService
    {
        private readonly CryptoPriceApiOptions _options;
        private IHttpClientFactory _httpClientFactory;

        public CryptoCompareClient(IHttpClientFactory httpClientFactory, IOptions<CryptoPriceApiOptions> options)
        {
            _options = options.Value;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CryptoPriceResponseDto> GetPriceInfo(string symbol)
        {
            ArgumentNullException.ThrowIfNull(symbol);

            var client = _httpClientFactory.CreateClient(ApiClient.CryptoPrice);
            var response = await client.GetAsync($"price?fsym={symbol.ToUpper()}&tsyms={_options.Currency}");
            var content = await response.HandleResponse();

            if(IsBadRequest(content)) //Crypto compare returns status code 200 even if provided symbol incorrect
            {
                return null;
            }

            var finalContent = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(content);

            return new CryptoPriceResponseDto
            {
                Currency = _options.Currency,
                Price = finalContent![_options.Currency],
                Symbol = symbol.ToUpper()
            };
        }

        public async Task<List<CryptoPriceResponseDto>> GetPriceInfo(List<string> symbols)
        {
            ArgumentNullException.ThrowIfNull(symbols);

            var client = _httpClientFactory.CreateClient(ApiClient.CryptoPrice);
            var response = await client.GetAsync($"pricemulti?fsyms={ConvertSymbolsArrayToString(symbols)}&tsyms={_options.Currency}");
            var content = await response.HandleResponse();

            if (IsBadRequest(content)) //Crypto compare returns status code 200 even if provided symbol incorrect
            {
                return null;
            }

            var finalContent = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, decimal>>>(content);

            var finalResult = new List<CryptoPriceResponseDto>();

            foreach(var item in finalContent!)
            {
                finalResult.Add(new CryptoPriceResponseDto
                {
                    Currency = _options.Currency,
                    Symbol = item.Key,
                    Price = item.Value[_options.Currency]
                });
            }

            return finalResult;
        }

        private bool IsBadRequest(string content)
        {
            if(content is null)
            {
                return true;
            }

            if(content.Contains("Response") && content.Contains("Error") && content.Contains("Message"))
            {
                return true;
            }

            return false;
        }

        private string ConvertSymbolsArrayToString(List<string> symbols)
        {
            return string.Join(",", symbols.Select(i => i.ToUpper()));
        }
    }
}
