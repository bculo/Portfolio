using Crypto.Application.Behaviours;
using Crypto.Application.Consumers;
using Crypto.Application.Consumers.State;
using Crypto.Application.Options;
using Crypto.Core.Exceptions;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            services.Configure<RedisOptions>(configuration.GetSection("RedisOptions"));
            services.Configure<SagaTimeoutOptions>(configuration.GetSection("SagaTimeoutOptions"));

            services.AddScoped<IDateTimeProvider, LocalDateTimeService>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddMediatR(typeof(ApplicationLayer).Assembly);
            services.AddAutoMapper(typeof(ApplicationLayer).Assembly);
            services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
        }

        public static void ConfigureMessageQueue(IServiceCollection services, IConfiguration configuration, bool useConsumers)
        {
            services.AddMassTransit(x =>
            {
                x.AddSagaStateMachine<AddCryptoItemStateMachine, AddCryptoItemState>()
                    .InMemoryRepository();

                x.AddDelayedMessageScheduler();

                if (useConsumers)
                {
                    ConfigureConsumers(x);
                }

                x.UsingRabbitMq((context, config) =>
                {
                    config.UseDelayedMessageScheduler();
                    config.Host(configuration["QueueOptions:Address"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }

        private static void ConfigureConsumers(IBusRegistrationConfigurator config)
        {
            config.AddConsumers(Assembly.GetExecutingAssembly());
        }
    }
}
