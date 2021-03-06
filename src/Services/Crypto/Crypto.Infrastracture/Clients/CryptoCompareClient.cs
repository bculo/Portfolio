using Crypto.Application.Interfaces.Services;
using Crypto.Application.Models.Price;
using Crypto.Application.Options;
using Http.Common.Extensions;
using HttpUtility.Abstract;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using String.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infrastracture.Clients
{
    public class CryptoCompareClient : BaseHttpClient, ICryptoPriceService
    {
        private readonly CryptoPriceApiOptions _options;

        public CryptoCompareClient(HttpClient client, IOptions<CryptoPriceApiOptions> options) : base(client)
        {
            _options = options.Value;

            AddHeader(_options.HeaderKey, _options.ApiKey);
        }

        public async Task<CryptoPriceResponseDto> GetPriceInfo(string symbol)
        {
            var url = $"{_options.BaseUrl}/price?fsym={symbol.ToUpper()}&tsyms={_options.Currency}";

            var response = await Client.GetAsync(url);

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
            var url = $"{_options.BaseUrl}/pricemulti?fsyms={ConvertSymbolsArrayToString(symbols)}&tsyms={_options.Currency}";

            var response = await Client.GetAsync(url);

            var content = await response.HandleResponse();

            if (IsBadRequest(content)) //Crypto compare returns status code 200 even if provided symbol incorrect
            {
                return null;
            }

            var finalContent = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, decimal>>>(content);

            var finalResult = new List<CryptoPriceResponseDto>();

            foreach(var item in finalContent)
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
