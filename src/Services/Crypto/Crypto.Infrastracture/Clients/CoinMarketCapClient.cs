using Crypto.Application.Interfaces.Services;
using Crypto.Application.Models.Info;
using Crypto.Application.Options;
using Crypto.Infrastracture.Constants;
using Http.Common.Extensions;
using Microsoft.Extensions.Options;

namespace Crypto.Infrastracture.Clients
{
    public class CoinMarketCapClient : ICryptoInfoService
    {
        private readonly CryptoInfoApiOptions _options;
        private readonly IHttpClientFactory _clientFactory;

        public CoinMarketCapClient(IHttpClientFactory clientFactory,
            IOptions<CryptoInfoApiOptions> options)
        {
            _options = options.Value;
            _clientFactory = clientFactory;
        }

        public async Task<CryptoInfoResponseDto> FetchData(string symbol)
        {
            ArgumentNullException.ThrowIfNull(symbol);
            var client = _clientFactory.CreateClient(ApiClient.CryptoInfo);
            var response = await client.GetAsync($"info?symbol={symbol}");
            return await response.HandleResponse<CryptoInfoResponseDto>();
        }
    }
}
