using Auth0.Abstract.Contracts;
using Keycloak.Common.Handlers;
using Keycloak.Common.Options;
using Keycloak.Common.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Refit;

namespace Keycloak.Common
{
    public static class KeycloakExtensions
    {
        public static void UseKeycloakClaimServices(this IServiceCollection services, 
            string keyCloackApplicationName)
        {
            ArgumentNullException.ThrowIfNull(keyCloackApplicationName);
            
            services.AddOptions<KeycloakClaimOptions>().Configure(opt =>
            {
                opt.ApplicationName = keyCloackApplicationName;
            });
            
            services.AddHttpContextAccessor();
            services.AddTransient<IClaimsTransformation, KeycloakClaimsTransformer>();
            services.AddScoped<IAuth0AccessTokenReader, KeycloakUserInfo>();
        }
        
        public static void UseKeycloakClientCredentialFlowService(this IServiceCollection services, 
            string authorizationServer)
        {
            ArgumentNullException.ThrowIfNull(authorizationServer);

            services.AddOptions<KeycloakTokenOptions>().Configure(opt =>
            {
                opt.AuthorizationServerUrl = authorizationServer;
            });

            services.AddScoped<IAuth0ClientCredentialFlowService, KeycloakClientCredentialFlowClient>();
        }

        public static void UseKeycloakOwnerPasswordCredentialFlowService(this IServiceCollection services,
            string authorizationUrl)
        {
            ArgumentNullException.ThrowIfNull(authorizationUrl);
            
            services.AddOptions<KeycloakTokenOptions>().Configure(opt =>
            {
                opt.AuthorizationServerUrl = authorizationUrl;
            });

            services.AddScoped<IAuth0ResourceOwnerPasswordFlowService, KeycloakResourceOwnerPasswordFlowClient>();
        }

        public static void RemoveClaimTransformerService(this IServiceCollection services)
        {
            var serviceDescriptor = services
                .FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IClaimsTransformation));
            services.Remove(serviceDescriptor);
        }
        
        public static void UseKeycloakAdminService(this IServiceCollection services, 
            string adminApiBase,
            string realm,
            string clientId,
            string clientSecrets, 
            string authorizationUrl, 
            string tokenUrl)
        {
            ArgumentNullException.ThrowIfNull(adminApiBase);
            ArgumentNullException.ThrowIfNull(realm);
            ArgumentNullException.ThrowIfNull(clientId);
            ArgumentNullException.ThrowIfNull(clientSecrets);
            ArgumentNullException.ThrowIfNull(authorizationUrl);
            ArgumentNullException.ThrowIfNull(tokenUrl);
            
            services.UseKeycloakClientCredentialFlowService(tokenUrl);
            
            services.AddOptions<KeycloakAdminApiOptions>().Configure(opt =>
            {
                opt.AdminApiBaseUri = adminApiBase;
                opt.Realm = realm;
                opt.AuthorizationUrl = authorizationUrl;
                opt.ClientId = clientId;
                opt.ClientSecret = clientSecrets;
                opt.TokenBaseUri = tokenUrl;
            });

            services.AddHttpContextAccessor();
            services.TryAddScoped<ClientCredentialAuthHandler>();
            
            services.AddRefitClient<IUsersApi>()
                .ConfigureHttpClient(c => c.BaseAddress = new Uri(adminApiBase))
                .AddHttpMessageHandler<ClientCredentialAuthHandler>()
                .AddPolicyHandler(
                    HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .WaitAndRetryAsync(
                            Backoff.DecorrelatedJitterBackoffV2(
                                TimeSpan.FromSeconds(0.5),
                                3)));
        }
    }
}
