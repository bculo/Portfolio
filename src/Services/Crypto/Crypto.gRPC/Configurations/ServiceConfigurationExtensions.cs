﻿using Crypto.Application;
using Crypto.Application.Common.Options;
using Crypto.Infrastructure;
using MassTransit;

namespace Crypto.gRPC.Configurations
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpc();
            services.AddGrpcReflection();

            ApplicationLayer.AddServices(services, configuration);

            InfrastructureLayer.AddServices(services, configuration);

            services.Configure<QueueOptions>(configuration.GetSection("QueueOptions"));
            services.AddMassTransit(x =>
            { 
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                });
            });
            
        }
    }
}
