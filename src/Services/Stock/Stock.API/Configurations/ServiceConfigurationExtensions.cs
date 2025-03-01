using Keycloak.Common;
using Serilog;
using Stock.API.Services;
using Stock.Application;
using Stock.Application.Interfaces.User;
using Stock.Infrastructure;
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
            
            services.ConfigureSwaggerWithApiVersioning(configuration["AuthOptions:ApplicationName"],
                $"{configuration["AuthOptions:AuthorizationServerUrl"]}/protocol/openid-connect/auth",
                configuration.GetValue<int>("ApiVersion:MajorVersion"),
                configuration.GetValue<int>("ApiVersion:MinorVersion"));

            ApplicationLayer.AddServices(services, configuration);
            InfrastructureLayer.AddServices(services, configuration);
            InfrastructureLayer.AddOpenTelemetry(services, configuration, "stock.api");
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
            services.UseKeycloakClaimServices(configuration["AuthOptions:ApplicationName"]);
            services.UseKeycloakClientCredentialFlowService(
                configuration["AuthOptions:AuthorizationServerUrl"],
                configuration["AuthOptions:RealmName"]);

            var authOptions = new AuthOptions();
            configuration.GetSection("AuthOptions").Bind(authOptions);

            services.ConfigureDefaultAuthentication(authOptions);
            services.ConfigureDefaultAuthorization();
        }
    }
}
