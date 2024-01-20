using System.Reflection;
using Cryptography.Common.Utils;
using Keycloak.Common;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Notification.Application;
using Notification.Application.Constants;
using Notification.Application.Consumers.Trend;
using Notification.Application.Interfaces;
using Notification.Hub.Services;
using WebProject.Common.Extensions;
using WebProject.Common.Options;

namespace Notification.Hub.Extensions;

public static class ConfigurationExtensions
{
    public static void ConfigureSignalRHubApp(this WebApplicationBuilder builder)
    {
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

    public static void RegisterServiceDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(ApplicationLayer).Assembly);
        });
        
        services.AddScoped<INotificationService, SignalRNotificationService>();
    }
    
    public static void ConfigureSignalR(IServiceCollection services, IConfiguration configuration)
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
                options.Configuration.ChannelPrefix = "SignalROptions:AppPrefix";
            });
    }
    
    public static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services.UseKeycloakClaimServices(configuration["KeycloakOptions:ApplicationName"]);

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
    
    public static void ConfigureMessageQueue(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            var formatter = new KebabCaseEndpointNameFormatter(prefix: MessageQueueConstants.QUEUE_PREFIX, false);
            x.SetEndpointNameFormatter(formatter);
            
            x.AddConsumer<SyncExecutedConsumer>();
            
            x.UsingRabbitMq((context, config) =>
            {
                config.Host(configuration["QueueOptions:Address"]);
                config.ConfigureEndpoints(context);
            });
        });
    }
}