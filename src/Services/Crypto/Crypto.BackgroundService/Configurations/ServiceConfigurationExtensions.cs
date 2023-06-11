using Cache.Common;
using Crypto.Application;
using Crypto.Application.Options;
using Crypto.BackgroundService.Interfaces;
using Crypto.BackgroundService.Jobs;
using Crypto.Infrastracture;
using Crypto.Infrastracture.Consumers;
using Crypto.Infrastracture.Persistence;
using Hangfire;
using Hangfire.SqlServer;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.BackgroundUpdate.Configurations
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfigureBackgroundService(this IServiceCollection services, IConfiguration configuration)
        {
            ApplicationLayer.AddServices(services, configuration);

            InfrastractureLayer.AddCommonServices(services, configuration);
            InfrastractureLayer.AddPersistenceStorage(services, configuration);
            CacheConfiguration.AddRedis(services, configuration);
            InfrastractureLayer.AddClients(services, configuration);

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
