using Auth0.Abstract.Contracts;
using Microsoft.Extensions.Options;
using Tracker.Application.Common.Constants;
using Tracker.Application.Common.Options;
using Tracker.Application.Interfaces;
using Tracker.Core.Exceptions;
using Http.Common.Extensions;

namespace Tracker.Infrastructure.Services
{
    public class CryptoHttpAssetClient : IFinancialAssetClient
    {
        private readonly IHttpClientFactory _factory;
        private readonly IAuth0ClientCredentialFlowService _clientCredential;
        private readonly ApplicationInfoOptions _infoOptions;

        public CryptoHttpAssetClient(IHttpClientFactory factory,
            IAuth0ClientCredentialFlowService clientCredential,
            IOptionsSnapshot<ApplicationInfoOptions> optionsSnapshot)
        {
            _factory = factory;
            _clientCredential = clientCredential;
            _infoOptions = optionsSnapshot.Value;
        }

        public async Task<FinancialAssetDto> FetchAsset(string symbol)
        {
            var client = _factory.CreateClient(HttpClientNames.CRYPTO_CLIENT);

            var accessToken = await _clientCredential.GetToken(_infoOptions.Name, _infoOptions.Secret);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken.AccessToken}");

            var httpResonse = await client.GetAsync($"v1/Crypto/Single/{symbol}");
            var content = await httpResonse.HandleResponse<CryptoResponse>();
            if (content is null)
            {
                throw new TrackerCoreException($"Item with symbol {symbol} not available");
            }

            return new FinancialAssetDto
            {
                Name = content.Name,
                Symbol = content.Symbol,
                Price = (float)content.Price,
            };
        }

        class CryptoResponse
        {
            public string Symbol { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
        }
    }
}
