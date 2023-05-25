using Keycloak.Common;
using MassTransit;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Tracker.API.Filters;
using Tracker.Application;
using Tracker.Application.Infrastructure.Consumers;
using WebProject.Common.Extensions;
using WebProject.Common.Options;
using WebProject.Common.Rest;

namespace Tracker.API.Configurations;

public static class ServiceConfigurationExtensions
{
    public static void ConfigureApiProject(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(opt =>
        {
            opt.Filters.Add<GlobalExceptionFilter>();
        });

        services.AddCors();

        ApplicationLayer.AddServices(services, configuration);
        ApplicationLayer.AddCache(services, configuration);
        
        services.AddMassTransit(x =>
        { 
            x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "Tracker", false));
            
            x.AddConsumer<CryptoPriceUpdatedConsumer>();
            x.AddConsumer<CryptoPriceUpdatedConsumer>();
            
            x.UsingRabbitMq((context, config) =>
            {
                config.Host(configuration["QueueOptions:Address"]);
                config.ConfigureEndpoints(context);
            });
        });
        
        services.ConfigureSwagger(
            $"{configuration["KeycloakOptions:AuthorizationServerUrl"]}/protocol/openid-connect/auth", 
            "Tracker.API");
        
        AddAuthentication(services, configuration);
        AddOpenTelemetry(services, configuration);
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
                        .AddService("Tracker.API"))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddJaegerExporter();
            });
    }
}