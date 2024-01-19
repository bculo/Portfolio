using System.Reflection;
using Cryptography.Common.Utils;
using Keycloak.Common;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Notification.Application.Interfaces;
using Notification.Hub.Services;

namespace Notification.Hub.Extensions;

public static class ConfigurationExtensions
{
    public static void ConfigureSignalRHubApp(this WebApplicationBuilder builder)
    {
        ConfigureAuthentication(builder.Services, builder.Configuration);
        RegisterServiceDependencies(builder.Services, builder.Configuration);
        ConfigureSignalR(builder.Services, builder.Configuration);
        ConfigureMessageQueue(builder.Services, builder.Configuration);
    }

    public static void RegisterServiceDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
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

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = configuration.GetValue<bool>("AuthOptions:ValidateAudience"),
                ValidateIssuer = configuration.GetValue<bool>("AuthOptions:ValidateIssuer"),
                ValidIssuers = new[] { configuration["AuthOptions:ValidIssuer"] },
                ValidateIssuerSigningKey = configuration.GetValue<bool>("AuthOptions:ValidateIssuerSigningKey"),
                IssuerSigningKey = RsaUtils.ImportSubjectPublicKeyInfo(configuration["AuthOptions:PublicRsaKey"]),
                ValidateLifetime = configuration.GetValue<bool>("AuthOptions:ValidateLifetime")
            };

            opt.Events = new JwtBearerEvents()
            {
                OnTokenValidated = context =>
                {
                    Console.WriteLine("User successfully authenticated");
                    return Task.CompletedTask;
                },

                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/portfolio")))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                },

                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("Failed authentication");
                    return Task.CompletedTask;
                }
            };
        });
    }
    
    public static void ConfigureMessageQueue(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, config) =>
            {
                config.Host(configuration["QueueOptions:Address"]);
                config.ConfigureEndpoints(context);
            });
        });
    }
}