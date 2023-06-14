using Crypto.gRPC.Protos.v1;
using Tracker.Application.Interfaces;

namespace Tracker.Infrastructure.Services
{
    public class CryptogRPCAssetClient : IFinancialAssetClient
    {
        private readonly Crypto.gRPC.Protos.v1.Crypto.CryptoClient _client;

        public CryptogRPCAssetClient(Crypto.gRPC.Protos.v1.Crypto.CryptoClient client)
        {
            _client = client;
        }

        public async Task<FinancialAssetDto> FetchAsset(string symbol)
        {
            ArgumentException.ThrowIfNullOrEmpty(symbol);

            var request = new FetchCryptoItemRequest
            {
                Symbol = symbol
            };

            var response = await _client.FetchCryptoItemAsync(request);

            return new FinancialAssetDto
            {
                Name = response.Name,
                Price = response.Price,
                Symbol = response.Symbol,
            };
        }
    }
}
