using System.Globalization;
using Keycloak.Common;
using MassTransit;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Trend.API.Services;
using Trend.Application;
using Trend.Application.Configurations.Constants;
using Trend.Application.Configurations.Options;
using Trend.Application.Consumers;
using Trend.Application.Interfaces;
using Trend.Application.Utils;
using WebProject.Common.CachePolicies;
using WebProject.Common.Extensions;
using WebProject.Common.Options;
using WebProject.Common.Rest;

namespace Trend.API.Extensions;

public static class ServiceConfigurationExtensions
{
    public static void ConfigureApiProject(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        
        if (builder.Environment.IsDevelopment())
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policyBuilder => policyBuilder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }
        
        services.AddControllers();
        services.AddProblemDetails();

        services.AddHealthChecks()
            .AddRedis(configuration["RedisOptions:ConnectionString"])
            .AddMongoDb(configuration["MongoOptions:ConnectionString"])
            .AddRabbitMQ(new Uri(configuration["QueueOptions:Address"]));

        ConfigureCache(services, configuration);
        AddMessageQueue(services, configuration);
        ConfigureLocalization(services);
        ConfigureAuthentication(services, configuration);

        services.ConfigureSwaggerWithApiVersioning(configuration["KeycloakOptions:ApplicationName"],
            $"{configuration["KeycloakOptions:AuthorizationServerUrl"]}/protocol/openid-connect/auth",
            configuration.GetValue<int>("ApiVersion:MajorVersion"),
            configuration.GetValue<int>("ApiVersion:MinorVersion"));
        
        ApplicationLayer.AddLogger(builder.Host);
        ApplicationLayer.AddServices(configuration, services);
        ApplicationLayer.ConfigureHangfire(configuration, services);
        ApplicationLayer.AddOpenTelemetry(configuration, services, "Trend.API");
    }

    private static void ConfigureLocalization(IServiceCollection services)
    {
        services.AddLocalization();

        services.Configure<RequestLocalizationOptions>(opts =>
        {
            var hrCulture = new CultureInfo("hr");
            var enCulture = new CultureInfo("en");
            var supportedCultures = new[]
            {
                hrCulture,
                enCulture
            };
            opts.DefaultRequestCulture = new RequestCulture(enCulture, enCulture);
            opts.SupportedCultures = supportedCultures;
            opts.SupportedUICultures = supportedCultures;
        });
    }

    private static void ConfigureCache(IServiceCollection services, IConfiguration configuration)
    {
        ApplicationLayer.ConfigureCache(configuration, services);
        services.AddOutputCache(opt =>
        {
            opt.AddBasePolicy(policy => policy
                .Expire(TimeSpan.FromSeconds(30)));
            
            opt.AddPolicy("NewsPolicy", policy => policy.AddPolicy<AuthGetRequestPolicy>()
                .Expire(TimeSpan.FromDays(1))
                .Tag(CacheTags.News));
            
            opt.AddPolicy("DictionaryPolicy", policy => policy.AddPolicy<AuthGetRequestPolicy>()
                .Expire(TimeSpan.FromDays(1))
                .Tag(CacheTags.Dictionary));

            opt.AddPolicy("SearchWordPolicy", policy => policy.AddPolicy<AuthGetRequestPolicy>()
                .Expire(TimeSpan.FromHours(2))
                .Tag(CacheTags.SearchWord));
            
            opt.AddPolicy("SyncPolicy", policy => policy.AddPolicy<AuthGetRequestPolicy>()
                .Expire(TimeSpan.FromHours(2))
                .Tag(CacheTags.Sync));
            
            opt.AddPolicy("SyncPostPolicy", policy => policy.AddPolicy<AuthPostRequestPolicy>()
                .Expire(TimeSpan.FromMinutes(30))
                .Tag(CacheTags.Sync));
            
            opt.AddPolicy("WordPostPolicy", policy => policy.AddPolicy<AuthPostRequestPolicy>()
                .Expire(TimeSpan.FromMinutes(30))
                .Tag(CacheTags.SearchWord));
        });
    }

    private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICurrentUser, UserService>();

        services.UseKeycloakClaimServices(configuration["KeycloakOptions:ApplicationName"]);
        services.UseKeycloakClientCredentialFlowService(configuration["KeycloakOptions:AuthorizationServerUrl"]);

        var authOptions = new AuthOptions();
        configuration.GetSection("AuthOptions").Bind(authOptions);

        services.ConfigureDefaultAuthentication(authOptions);
        services.ConfigureDefaultAuthorization();
    }

    private static void AddMessageQueue(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "Trend", false));

            x.AddConsumer<SyncExecutedConsumer, SyncExecutedConsumerDefinition>();
            
            x.UsingRabbitMq((context, config) =>
            {
                config.Host(configuration["QueueOptions:Address"]);
                config.ConfigureEndpoints(context);
            });
        });
    }
    
    public static async Task SeedAll(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        await using var provider = services.BuildServiceProvider();
        await using var scope = provider.CreateAsyncScope();

        var blobStorage = scope.ServiceProvider.GetRequiredService<IBlobStorage>();
        var blobOptions = scope.ServiceProvider.GetRequiredService<IOptions<BlobStorageOptions>>();
        await StorageSeedUtils.SeedBlobStorage(blobStorage, blobOptions);
    }
}

