using Crypto.Application.Interfaces.Services;
using Crypto.Application.Models.Info;
using Crypto.Application.Options;
using Http.Common.Extensions;
using HttpUtility.Abstract;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infrastracture.Clients
{
    public class CoinMarketCapClient : BaseHttpClient, ICryptoInfoService
    {
        private readonly CryptoInfoApiOptions _options;

        public CoinMarketCapClient(HttpClient client, IOptions<CryptoInfoApiOptions> options) : base(client)
        {
            _options = options.Value;
        }

        public async Task<CryptoInfoResponseDto> FetchData(string symbol)
        {
            AddHeader(_options.HeaderKey, _options.ApiKey);

            var response = await Client.GetAsync($"{_options.BaseUrl}info?symbol={symbol}");

            return await response.HandleResponse<CryptoInfoResponseDto>();
        }
    }
}
