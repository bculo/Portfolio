using Hangfire;
using Hangfire.PostgreSql;
using Stock.Application;
using Stock.Application.Interfaces;
using Stock.Worker.Jobs;
using Stock.Worker.Services;

namespace Stock.Worker.Configurations
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfigureBackgroundService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IStockUser, WorkerUserService>();

            ApplicationLayer.AddPersistence(services, configuration);
            ApplicationLayer.AddServices(services, configuration);
            ApplicationLayer.AddClients(services, configuration);

            ConfigureHangfire(services, configuration);
        }

        private static void ConfigureHangfire(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
                config.UseSimpleAssemblyNameTypeSerializer();
                config.UseRecommendedSerializerSettings();
                config.UsePostgreSqlStorage(configuration.GetConnectionString("StockDatabase"));
            });

            services.AddHangfireServer();

            services.AddScoped<IPriceUpdateJobService, UpdateStockPriceHangfireJob>();
        }
    }
}
