using Carter;
using Keycloak.Common;
using Keycloak.Common.Utils;
using Mail.Application;
using Mail.Application.Consumers;
using Mail.Infrastructure;
using MassTransit;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using WebProject.Common.Extensions;
using WebProject.Common.Options;
using WebProject.Common.Rest;

namespace Mail.API.Extensions;

public static class ServiceConfigurationExtensions
{
    public static void ConfigureMinimalApiProject(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter();
        var authEndpoint = KeycloakUriUtils.BuildAuthEndpoint(
            configuration["AuthOptions:AuthorizationServerUrl"],
            configuration["AuthOptions:RealmName"]);
        services.ConfigureSwagger(authEndpoint);
        
        ApplicationLayer.AddServices(services, configuration);
        InfrastructureLayer.AddServices(services, configuration);
        
        AddAuthentication(services, configuration);
        AddOpenTelemetry(services, configuration);
        AddMessageBroker(services, configuration);
    }

    private static void AddMessageBroker(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "Mail", false));
            
            x.AddConsumer<SentimentCheckedConsumer, SentimentCheckedConsumerDefinition>();
            
            x.UsingRabbitMq((context, config) =>
            {
                config.Host(configuration["QueueOptions:Address"]);
                config.ConfigureEndpoints(context);
            });
        });
    }
    
    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services.UseKeycloakClaimServices(configuration["AuthOptions:ApplicationName"]);
        // services.UseKeycloakCredentialFlowService(configuration["AuthOptions:AuthorizationServerUrl"]);

        var authOptions = new AuthOptions();
        configuration.GetSection("AuthOptions").Bind(authOptions);

        services.ConfigureDefaultAuthentication(authOptions);
        services.ConfigureDefaultAuthorization();
    }
    
    private static void AddOpenTelemetry(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder
                    .AddSource("MassTransit")
                    .SetResourceBuilder(ResourceBuilder.CreateDefault()
                        .AddService("Mail.API"))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
            });
    }
}