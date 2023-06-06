using Hangfire;
using Hangfire.MemoryStorage;
using Stock.Application;
using Stock.Worker.Jobs;

namespace Stock.Worker.Configurations
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfigureBackgroundService(this IServiceCollection services, IConfiguration configuration)
        {
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
                config.UseMemoryStorage();
            });

            services.AddHangfireServer();

            services.AddScoped<IPriceUpdateJobService, UpdateStockPriceHangfireJob>();
        }
    }
}
