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
            services.AddStockGrpcClientFactory(configuration["IntegrationEndpoints:Stock"]!);
            services.AddScoped<StockAdapter>();
            services.AddScoped<CryptoAdapter>();
            services.AddScoped<IFinancialAssetAdapterFactory, FinancialAssetAdapterFactory>();
        }
        
        private static void AddPersistenceStorage(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TrackerDbContext>();
        }
    }
}
