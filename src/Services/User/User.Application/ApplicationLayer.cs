using Azure.Storage.Blobs;
using FluentValidation;
using Keycloak.Common;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Time.Common;
using User.Application.Common.Behaviours;
using User.Application.Common.Options;
using User.Application.Consumers;
using User.Application.Interfaces;
using User.Application.Services;

namespace User.Application
{
    public static class ApplicationLayer
    {
        public static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddLocalTimeProvider();
            services.AddScoped<IImageService, MagickImageService>();
            
            services.AddSingleton<IBlobStorage, BlobStorage>((provider) =>
            {
                var options = provider.GetRequiredService<IOptions<BlobStorageOptions>>();
                var logger = provider.GetRequiredService<ILogger<BlobStorage>>();
                var storage = new BlobStorage(new BlobServiceClient(options.Value.ConnectionString), options, logger);
                
                storage.InitializeContext(options.Value.ProfileContainerName);
                storage.InitializeContext(options.Value.VerificationContainerName);
                
                return storage;
            });
            
            services.Configure<BlobStorageOptions>(configuration.GetSection("BlobStorageOptions"));
            
            services.AddMediatR(opt =>
            {
                opt.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
                opt.RegisterServicesFromAssembly(typeof(ApplicationLayer).Assembly);
            });
            
            services.AddValidatorsFromAssembly(typeof(ApplicationLayer).Assembly);
            
            services.UseKeycloakAdminService(configuration["KeycloakAdminApiOptions:AdminApiBaseUri"]!,
                configuration["KeycloakAdminApiOptions:Realm"]!,
                configuration["KeycloakAdminApiOptions:ClientId"]!,
                configuration["KeycloakAdminApiOptions:ClientSecret"]!,
                configuration["KeycloakAdminApiOptions:AuthorizationUrl"]!,
                configuration["KeycloakAdminApiOptions:TokenBaseUri"]!);
            
            RegisterMessageBroker(services, configuration);
        }

        private static void RegisterMessageBroker(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix: "User", false));

                x.AddConsumer<DeleteKeycloakUserConsumer>();
                x.AddConsumer<UserImageVerifiedConsumer, UserImageVerifiedConsumerDefinition>();
                
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(configuration["QueueOptions:Address"]);
                    config.ConfigureEndpoints(context);
                });
            });
        }
    }
}
