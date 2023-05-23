using Crypto.gRPC.Protos.v1;
using Grpc.Net.Client;
using Tracker.Application.Interfaces;

namespace Tracker.Application.Infrastructure.Services;

public class CryptoAssetClient : IFinancialAssetClient
{
    public async Task<FinancialAssetDto> FetchAsset(string symbol)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(symbol);
        
        var channel = GrpcChannel.ForAddress("http://localhost:5098"); //TODO: use configuration and yarp endpoint
        var client = new Crypto.gRPC.Protos.v1.Crypto.CryptoClient(channel);

        var request = new FetchCryptoItemRequest
        {
            Symbol = symbol
        };
        
        var response = await client.FetchCryptoItemAsync(request);

        return new FinancialAssetDto
        {
            Name = response.Name,
            Price = response.Price,
            Symbol = response.Symbol,
        };
    }
}