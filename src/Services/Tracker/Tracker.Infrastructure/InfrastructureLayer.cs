using Crypto.API.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.gRPC.Client;
using Tracker.Application.Interfaces.Integration;
using Tracker.Infrastructure.Integration;
using Tracker.Infrastructure.Persistence;

namespace Tracker.Infrastructure
{
    public static class InfrastructureLayer
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<StockAdapter>();
            services.AddStockGrpcClientFactory(configuration["IntegrationEndpoints:Stock"]!);
            
            services.AddScoped<CryptoAdapter>();
            services.AddCryptoApiClients(configuration["IntegrationEndpoints:Crypto"]!);
            
            services.AddScoped<IFinancialAssetAdapterFactory, FinancialAssetAdapterFactory>();
        }
        
        private static void AddPersistenceStorage(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TrackerDbContext>();
        }
    }
}
