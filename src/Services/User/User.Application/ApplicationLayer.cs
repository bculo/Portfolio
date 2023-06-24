using FluentValidation;
using Keycloak.Common;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Time.Abstract.Contracts;
using Time.Common;
using User.Application.Common.Options;
using User.Application.Consumers;
using User.Application.Interfaces;
using User.Application.Persistence;
using User.Application.Services;

namespace User.Application
{
    public static class ApplicationLayer
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UserDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("UserDb"));
                options.UseLowerCaseNamingConvention();
            });

            services.Configure<KeycloakAdminOptions>(configuration.GetSection(nameof(KeycloakAdminOptions)));

            services.AddHttpClient();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());    

            services.AddScoped<IRegisterUserService, RegisterUserService>();
            services.AddScoped<IDateTimeProvider, UtcDateTimeService>();

            services.UseKeycloakAdminService(configuration["KeycloakAdminOptions:AdminApiBaseUri"]);
            services.UseKeycloakOwnerCredentialFlowService(configuration["KeycloakAdminOptions:TokenBaseUri"]);

            RegisterMessageBroker(services, configuration);
        }

        private static void RegisterMessageBroker(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "User", false));

                x.AddConsumer<UserNotSavedToPersistenceStorageConsumer>();

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }
    }
}
