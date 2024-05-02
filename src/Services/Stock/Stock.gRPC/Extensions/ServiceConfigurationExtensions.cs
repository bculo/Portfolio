using Keycloak.Common;
using MassTransit;
using Serilog;
using Stock.Application;
using Stock.Application.Interfaces.User;
using Stock.gRPC.Handlers;
using Stock.gRPC.Services;
using Stock.Infrastructure;
using WebProject.Common.Extensions;
using WebProject.Common.Options;

namespace Stock.gRPC.Extensions;

public static class ServiceConfigurationExtensions
{
    public static void ConfigureGrpcProject(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<GlobalExceptionHandler>();
        });
        services.AddGrpcReflection();
        
        services.AddScoped<IStockUser, UserService>();
        
        ApplicationLayer.AddServices(services, configuration);
        InfrastructureLayer.AddServices(services, configuration);
        InfrastructureLayer.AddOpenTelemetry(services, configuration, "stock.grpc");
        InfrastructureLayer.AddMessageQueue(services, configuration);
        
        AddAuthentication(services, configuration);
        AddSerilog(builder);
    }

    private static void AddSerilog(WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((host, log) =>
        {
            log.ReadFrom.Configuration(host.Configuration);
        });
    }
    
    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services.UseKeycloakClaimServices(configuration["KeycloakOptions:ApplicationName"]);
        services.UseKeycloakClientCredentialFlowService(configuration["KeycloakOptions:AuthorizationServerUrl"]);

        var authOptions = new AuthOptions();
        configuration.GetSection("AuthOptions").Bind(authOptions);

        services.ConfigureDefaultAuthentication(authOptions);
        services.ConfigureDefaultAuthorization();
    }
}