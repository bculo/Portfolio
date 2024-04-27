using Events.Common.Trend;
using Keycloak.Common;
using MassTransit;
using Notification.Application;
using Notification.Application.Features.Crypto;
using Notification.Application.Features.Trend;
using Notification.Application.Interfaces.Notifications;
using Notification.Hub.Services;
using Time.Common;
using WebProject.Common.Extensions;
using WebProject.Common.Options;

namespace Notification.Hub.Extensions;

public static class ConfigurationExtensions
{
    public static void ConfigureSignalRHubApp(this WebApplicationBuilder builder)
    {
        builder.Services.AddUtcTimeProvider();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policyBuilder => policyBuilder
                .WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        });
        
        ConfigureAuthentication(builder.Services, builder.Configuration);
        RegisterServiceDependencies(builder.Services, builder.Configuration);
        ConfigureSignalR(builder.Services, builder.Configuration);
        ConfigureMessageQueue(builder.Services, builder.Configuration);
    }

    private static void RegisterServiceDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(ApplicationLayer).Assembly);
        });
        
        services.AddScoped<INotificationService, SignalRNotificationService>();
    }
    
    private static void ConfigureSignalR(IServiceCollection services, IConfiguration configuration)
    {
        if (!configuration.GetValue<bool>("SignalROptions:UseRedis"))
        {
            services.AddSignalR();
            return;
        }
        
        string connectionString = configuration["SignalROptions:RedisConnection"]
            ?? throw new ArgumentNullException(nameof(connectionString));
        
        services.AddSignalR()
            .AddStackExchangeRedis(connectionString, options => 
            {
                options.Configuration.ChannelPrefix = configuration["SignalROptions:AppPrefix"];
            });
    }
    
    private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services.UseKeycloakClaimServices(configuration["KeycloakOptions:ApplicationName"]!);

        var authOptions = new AuthOptions();
        configuration.GetSection("AuthOptions").Bind(authOptions);
        
        services.ConfigureDefaultAuthentication(authOptions, onMessageReceived: context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/portfolio")))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        });
        
        services.ConfigureDefaultAuthorization();
    }
    
    private static void ConfigureMessageQueue(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            var formatter = new KebabCaseEndpointNameFormatter(prefix: "notification", false);
            x.SetEndpointNameFormatter(formatter);

            x.ConfigureTrendConsumers();
            
            x.AddConsumer<CryptoPriceUpdatedConsumer>();
            
            x.UsingRabbitMq((context, config) =>
            {
                config.Host(configuration["QueueOptions:Address"]);
                config.ConfigureEndpoints(context);
            });
        });
    }
}