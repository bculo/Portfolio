﻿using FluentValidation;
using Keycloak.Common;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using Time.Abstract.Contracts;
using Time.Common;
using User.Application.Common.Behaviours;
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
            
            services.AddHttpClient();

            services.AddScoped<IUserManagerService, UserManagerService>();
            services.AddScoped<IDateTimeProvider, UtcDateTimeService>();

            services.AddMediatR(opt =>
            {
                opt.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
                opt.RegisterServicesFromAssembly(typeof(ApplicationLayer).Assembly);
            });
            
            services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
            
            services.UseKeycloakAdminService(configuration["KeycloakAdminApiOptions:AdminApiBaseUri"],
                configuration["KeycloakAdminApiOptions:Realm"],
                configuration["KeycloakAdminApiOptions:ClientId"],
                configuration["KeycloakAdminApiOptions:ClientSecret"],
                configuration["KeycloakAdminApiOptions:AuthorizationUrl"],
                configuration["KeycloakAdminApiOptions:TokenBaseUri"]);
            
            RegisterMessageBroker(services, configuration);
        }

        private static void RegisterMessageBroker(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "User", false));

                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }
    }
}
