﻿using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Cache.Common;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StackExchange.Redis;
using Trend.Application;
using Trend.Application.Configurations.Options;
using Trend.Application.Consumers;
using Trend.Domain.Interfaces;

namespace Trend.BackgroundSync.Extensions
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfigureBackgroundService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SyncBackgroundServiceOptions>(configuration.GetSection("SyncBackgroundServiceOptions"));

            services.AddOutputCache();
            var redisConnectionString = configuration["RedisOptions:ConnectionString"];
            var multiplexer = CacheConfiguration.AddConnectionMultiplexer(services, redisConnectionString);
            services.AddStackExchangeRedisOutputCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = configuration["RedisOptions:InstanceName"];
                options.ConnectionMultiplexerFactory = () => Task.FromResult(multiplexer);
            });

            ApplicationLayer.AddClients(configuration, services);
            ApplicationLayer.AddServices(configuration, services);
            ApplicationLayer.AddPersistence(configuration, services);
            ApplicationLayer.ConfigureHangfire(configuration, services, true);

            AddMessageQueue(services, configuration);
            AddOpenTelemetry(services, configuration, multiplexer);
        }

        private static void AddMessageQueue(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "Trend", false));

                x.AddConsumer<ExecuteNewsSyncConsumer>();

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }
        
        private static void AddOpenTelemetry(IServiceCollection services, 
            IConfiguration config,
            IConnectionMultiplexer multiplexer = null)
        {
            services.AddOpenTelemetry()
                .ConfigureResource(resource =>
                {
                    resource.AddService("Trend.BackgroundSync");
                })
                .WithMetrics(metrics => metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddMeter("Microsoft.AspNetCore.Hosting")
                    .AddMeter("Microsoft.AspNetCore.Server.Kestrel"))
                .WithTracing(tracing =>
                {
                    tracing.AddSource("MassTransit");
                    tracing.AddMongoDBInstrumentation();
                    tracing.AddAspNetCoreInstrumentation();
                    tracing.AddHttpClientInstrumentation();

                    if (multiplexer is not null)
                    {
                        tracing.AddRedisInstrumentation(multiplexer);
                    }

                    tracing.AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri(config["OpenTelemetry:OtlpExporter"] 
                                               ?? throw new ArgumentNullException());
                    });
                    tracing.AddConsoleExporter();
                });
        }

    }
}
