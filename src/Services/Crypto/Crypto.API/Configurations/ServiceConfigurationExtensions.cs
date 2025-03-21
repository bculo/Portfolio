﻿using Crypto.API.Handlers;
using Crypto.Application;
using Crypto.Application.Common.Options;
using Crypto.Infrastructure;
using Crypto.Infrastructure.Consumers;
using Crypto.Infrastructure.Consumers.State;
using Crypto.Infrastructure.Extensions;
using Keycloak.Common;
using Keycloak.Common.Utils;
using MassTransit;
using WebProject.Common.Extensions;
using WebProject.Common.Options;
using WebProject.Common.Rest;

namespace Crypto.API.Configurations;

public static class ServiceConfigurationExtensions
{
    public static void ConfigureApiProject(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        });

        services.AddControllers();
        services.AddCors();

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        ApplicationLayer.AddServices(services, configuration);
        InfrastructureLayer.AddServices(services, configuration);

        var authEndpoint = KeycloakUriUtils.BuildAuthEndpoint(
            configuration["AuthOptions:AuthorizationServerUrl"],
            configuration["AuthOptions:RealmName"]);
        services.ConfigureSwaggerAsEndpoints(authEndpoint);

        AddAuthentication(services, configuration);
        AddMessageQueue(services, configuration);
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services.UseKeycloakClaimServices(configuration["AuthOptions:ApplicationName"]);
        services.UseKeycloakClientCredentialFlowService(
            configuration["AuthOptions:AuthorizationServerUrl"],
            configuration["AuthOptions:RealmName"]);

        var authOptions = new AuthOptions();
        configuration.GetSection("AuthOptions").Bind(authOptions);

        services.ConfigureDefaultAuthentication(authOptions);
        services.ConfigureDefaultAuthorization();
    }

    private static void AddMessageQueue(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SagaTimeoutOptions>(configuration.GetSection("SagaTimeoutOptions"));
        services.AddMassTransit(GetMassTransitConfig(configuration));
    }

    public static Action<IBusRegistrationConfigurator> GetMassTransitConfig(IConfiguration configuration)
    {
        return x =>
        {
            var isTemporary = configuration.GetValue<bool>("QueueOptions:Temporary");

            x.AddDelayedMessageScheduler();
            x.SetEndpointNameFormatter(
                new KebabCaseEndpointNameFormatter(prefix: configuration["QueueOptions:Prefix"], false));

            x.AddStateMachine<AddCryptoItemStateMachine, AddCryptoItemState>(isTemporary);

            x.AddQueueConsumer<AddCryptoItemConsumer>(isTemporary);
            x.AddQueueConsumer<CryptoVisitedConsumer>(isTemporary);
            x.AddQueueConsumer<EvictRedisListRequestConsumer>(isTemporary);
            x.AddQueueConsumer<PriceUpdatedConsumer>(isTemporary);

            x.UsingRabbitMq((context, config) =>
            {
                config.UseDelayedMessageScheduler();
                config.Host(configuration["QueueOptions:Host"], configuration["QueueOptions:VirtualHost"], h =>
                {
                    h.Username(configuration["QueueOptions:Username"]);
                    h.Password(configuration["QueueOptions:Password"]);
                });
                config.ConfigureEndpoints(context);
            });
        };
    }
};
