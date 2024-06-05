using Keycloak.Common;
using Serilog;
using Stock.API.Common.CachePolicies;
using Stock.API.Common.Constants;
using Stock.API.Services;
using Stock.Application;
using Stock.Application.Common.Constants;
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
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
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
            InfrastructureLayer.AddOpenTelemetry(services, configuration, "stock.api");
            InfrastructureLayer.AddMessageQueue(services, configuration);
            
            AddAuthentication(services, configuration);
            AddCachePolicies(services, configuration);
            AddSerilog(builder);
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
                opt.AddBasePolicy(policy => policy.Tag(CacheTags.All));

                opt.AddPolicy(CachePolicies.StockGetFilter, policy => policy.AddPolicy<AuthGetRequestPolicy>()
                    .Expire(TimeSpan.FromMinutes(30))
                    .Tag(CacheTags.StockFilter));
                
                opt.AddPolicy(CachePolicies.StockGetSingle, policy => policy.AddPolicy<GetStockRequestPolicy>()
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
    }
}
