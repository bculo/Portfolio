using Crypto.Application.Behaviours;
using Crypto.Application.Consumers;
using Crypto.Application.Options;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time.Common;
using Time.Common.Contracts;

namespace Crypto.Application
{
    public class ApplicationLayer
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<QueueOptions>(configuration.GetSection("QueueOptions"));
            services.Configure<CryptoUpdateOptions>(configuration.GetSection("CryptoUpdateOptions"));

            services.AddScoped<IDateTime, LocalDateTimeService>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddMediatR(typeof(ApplicationLayer).Assembly);
            services.AddAutoMapper(typeof(ApplicationLayer).Assembly);
            services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
        }

        public static void ConfigureMessageQueue(IServiceCollection services, IConfiguration configuration, bool useConsumers)
        {
            services.AddMassTransit(x =>
            {
                if (useConsumers)
                {
                    ConfigureConsumers(x);
                }

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }

        private static void ConfigureConsumers(IBusRegistrationConfigurator config)
        {
            config.AddConsumer<CryptoPriceUpdatedConsumer>()
                .Endpoint(config =>
                {
                    config.Name = $"CRYPTO-{nameof(CryptoPriceUpdatedConsumer)}";
                });

            config.AddConsumer<CryptoVisitedConsumer>()
                .Endpoint(config =>
                {
                    config.Name = $"CRYPTO-{nameof(CryptoVisitedConsumer)}";
                });
        }
    }
}
