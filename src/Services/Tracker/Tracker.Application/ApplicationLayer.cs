using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Time.Common;
using Time.Common.Contracts;
using Tracker.Application.Infrastructure.Services;
using Tracker.Application.Interfaces;
using Tracker.Application.Options;

namespace Tracker.Application;

public static class ApplicationLayer
{
    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EndpointgRPCOptions>(configuration.GetSection(nameof(EndpointgRPCOptions)));
        
        services.AddScoped(services =>
        {
            var config = services.GetRequiredService<IOptionsSnapshot<EndpointgRPCOptions>>().Value;
            var channel = GrpcChannel.ForAddress(config.CryptoEndpoint);
            return new CryptogRPCAssetClient(new Crypto.gRPC.Protos.v1.Crypto.CryptoClient(channel));
        });
        
        services.AddScoped<IDateTimeProvider, LocalDateTimeService>();
        services.AddScoped<IFinancialAssetClientFactory, FinancialAssetClientFactory>();
    }
}