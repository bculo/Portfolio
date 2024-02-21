using Crypto.Application;
using Crypto.Application.Common.Options;
using Crypto.Infrastructure;
using Crypto.Infrastructure.Consumers;
using Crypto.Infrastructure.Persistence;
using Crypto.Worker.Interfaces;
using Crypto.Worker.Jobs;
using Hangfire;
using MassTransit;

namespace Crypto.Worker.Configurations
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfigureBackgroundService(this IServiceCollection services, IConfiguration configuration)
        {
            ApplicationLayer.AddServices(services, configuration);

            InfrastructureLayer.AddServices(services, configuration);

            services.Configure<QueueOptions>(configuration.GetSection("QueueOptions"));
            services.Configure<CryptoUpdateOptions>(configuration.GetSection("CryptoUpdateOptions"));

            ConfigureHangfire(services, configuration);

            services.AddMassTransit(x =>
            {
                x.AddEntityFrameworkOutbox<CryptoDbContext>(o =>
                {
                    o.UseSqlServer();
                    o.UseBusOutbox();
                });

                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "Crypto", false));

                x.AddConsumer<UpdateCryptoItemsPriceConsumer>();
                x.AddConsumer<CryptoPriceUpdatedConsumer>();

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }

        private static void ConfigureHangfire(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170);
                config.UseSimpleAssemblyNameTypeSerializer();
                config.UseRecommendedSerializerSettings();
                config.UseSqlServerStorage(configuration["ConnectionStrings:CryptoDatabase"]);
            });

            services.AddHangfireServer();

            services.AddScoped<IPriceUpdateJobService, PriceUpdateJobService>();
        }
    }
}
