using Cache.Common;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Time.Abstract.Contracts;
using Time.Common;
using Tracker.Application.Common.Constants;
using Tracker.Application.Common.Options;
using Tracker.Application.Interfaces;
using Tracker.Infrastructure.Persistence;
using Tracker.Infrastructure.Services;

namespace Tracker.Infrastructure
{
    public static class InfrastructureLayer
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EndpointgRPCOptions>(configuration.GetSection(nameof(EndpointgRPCOptions)));
            services.Configure<ApplicationInfoOptions>(configuration.GetSection(nameof(ApplicationInfoOptions)));

            
            services.AddScoped(services =>
            {
                var config = services.GetRequiredService<IOptionsSnapshot<EndpointgRPCOptions>>().Value;
                var channel = GrpcChannel.ForAddress(config.CryptoEndpoint);
                return new CryptogRPCAssetClient(new Crypto.gRPC.Protos.v1.Crypto.CryptoClient(channel));
            });
            

            services.AddScoped<IDateTimeProvider, LocalDateTimeService>();
            services.AddScoped<IFinancialAssetClientFactory, FinancialAssetClientFactory>();

            AddClients(services, configuration);

            CacheConfiguration.AddRedis(services, configuration);
        }

        private static void AddClients(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<CryptoHttpAssetClient>();
            services.AddHttpClient(HttpClientNames.CRYPTO_CLIENT, client =>
            {
                client.BaseAddress = new Uri("http://localhost:5263/api/");
                client.Timeout = TimeSpan.FromSeconds(60);
            })
            .AddTransientHttpErrorPolicy(policyBuilder =>
            {
                return policyBuilder.WaitAndRetryAsync(
                    Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromSeconds(1), 2));
            });
        }

        private static void AddPersistenceStorage(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TrackerDbContext>();
        }
    }
}
