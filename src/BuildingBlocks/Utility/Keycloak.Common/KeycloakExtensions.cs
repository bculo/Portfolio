using Ardalis.GuardClauses;
using Auth0.Abstract.Contracts;
using Keycloak.Common.Clients;
using Keycloak.Common.Options;
using Keycloak.Common.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Keycloak.Common
{
    /// <summary>
    /// Keycloak extensions
    /// </summary>
    public static class KeycloakExtensions
    {
        /// <summary>
        /// Register claims transformation and claim reader services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="keycloackApplicationName"></param>
        public static void UseKeycloakClaimServices(this IServiceCollection services, string keycloackApplicationName)
        {
            Guard.Against.NullOrEmpty(keycloackApplicationName);

            //Claim transformation
            services.AddOptions<KeycloakClaimOptions>().Configure(opt =>
            {
                opt.ApplicationName = keycloackApplicationName;
            });

            services.AddTransient<IClaimsTransformation, KeycloakClaimsTransformer>();

            //Reading claims
            services.AddHttpContextAccessor();
            services.AddScoped<IAuth0AccessTokenReader, KeycloakUserInfo>();
        }

        /// <summary>
        /// Register keycloak client for oauth flows
        /// </summary>
        /// <param name="services"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="tokenEndpoint"></param>
        public static void UseKeycloakFlowService(this IServiceCollection services,
            string clientId,
            string clientSecret,
            string tokenEndpoint)
        {
            Guard.Against.NullOrEmpty(clientId);
            Guard.Against.NullOrEmpty(clientSecret);
            Guard.Against.NullOrEmpty(tokenEndpoint);

            services.AddOptions<KeycloackClientCredentialFlowOptions>().Configure(opt =>
            {
                opt.ClientId = clientId;
                opt.ClientSecret = clientSecret;
                opt.AuthorizationServerUrl = tokenEndpoint;
            });

            services.AddHttpClient<IAuth0ClientCredentialFlowService, KeycloakClientCredentialFlowClient>();
        }
    }
}
