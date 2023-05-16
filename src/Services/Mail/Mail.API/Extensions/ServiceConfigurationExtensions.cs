using Carter;
using Keycloak.Common;
using Mail.Application;
using Mail.Application.Consumers;
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
        services.ConfigureSwagger($"{configuration["KeycloakOptions:AuthorizationServerUrl"]}/protocol/openid-connect/auth");
        
        ApplicationLayer.AddServices(services, configuration);
        
        AddAuthentication(services, configuration);
        AddOpenTelemetry(services, configuration);
        AddMessageBroker(services, configuration);
    }

    private static void AddMessageBroker(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "Mail", false));

            x.AddConsumer<SendCustomMailConsumer>();
            
            x.UsingRabbitMq((context, config) =>
            {
                config.Host(configuration["QueueOptions:Address"]);
                config.ConfigureEndpoints(context);
            });
        });
    }
    
    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services.UseKeycloakClaimServices(configuration["KeycloakOptions:ApplicationName"]);
        services.UseKeycloakCredentialFlowService(configuration["KeycloakOptions:AuthorizationServerUrl"]);

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
                    .AddHttpClientInstrumentation()
                    .AddJaegerExporter();
            });
    }
}