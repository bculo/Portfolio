using Keycloak.Common;
using MassTransit;
using Serilog;
using Stock.API.Common.CachePolicies;
using Stock.API.Common.Constants;
using Stock.API.Services;
using Stock.Application;
using Stock.Application.Common.Configurations;
using Stock.Application.Interfaces.User;
using Stock.Infrastructure;
using WebProject.Common.CachePolicies;
using WebProject.Common.Extensions;
using WebProject.Common.Options;
using WebProject.Common.Rest;

namespace Stock.API.Configurations
{
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
                        .WithOrigins("http://127.0.0.1:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
                });
            }
            
            services.AddHealthChecks()
                .AddRedis(configuration["RedisOptions:ConnectionString"])
                .AddNpgSql(configuration["ConnectionStrings:StockDatabase"])
                .AddRabbitMQ(new Uri(configuration["QueueOptions:Address"]));
            
            services.AddControllers();
            services.AddScoped<IStockUser, CurrentUserService>();
            
            services.ConfigureSwaggerWithApiVersioning(configuration["KeycloakOptions:ApplicationName"],
                $"{configuration["KeycloakOptions:AuthorizationServerUrl"]}/protocol/openid-connect/auth",
                configuration.GetValue<int>("ApiVersion:MajorVersion"),
                configuration.GetValue<int>("ApiVersion:MinorVersion"));

            ApplicationLayer.AddServices(services, configuration);
            InfrastructureLayer.AddServices(services, configuration);

            AddMessageQueue(services, configuration);
            AddAuthentication(services, configuration);
            AddCachePolicies(services, configuration);
            AddSerilog(builder);
            AddOpenTelemetry(services, configuration);
        }

        private static void AddSerilog(WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((host, log) =>
            {
                log.ReadFrom.Configuration(host.Configuration);
            });
        }
        
        private static void AddCachePolicies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOutputCache(opt =>
            {
                opt.AddBasePolicy(policy => policy.Tag(CacheTags.ALL));

                opt.AddPolicy(CachePolicies.STOCK_GET_FILTER, policy => policy.AddPolicy<AuthGetRequestPolicy>()
                    .Expire(TimeSpan.FromDays(1))
                    .Tag(CacheTags.STOCK_FILTER));
                
                opt.AddPolicy(CachePolicies.STOCK_GET_SINGLE, policy => policy.AddPolicy<GetStockRequestPolicy>()
                    .Expire(TimeSpan.FromHours(3)));
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

        private static void AddMessageQueue(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((_, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                });
            });
        }

        private static void AddOpenTelemetry(IServiceCollection services, IConfiguration configuration)
        {

        }
    }
}
