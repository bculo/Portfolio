using Crypto.gRPC.Protos.v1;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using Tracker.Application.Interfaces;
using Tracker.Application.Options;

namespace Tracker.Application.Infrastructure.Services;

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