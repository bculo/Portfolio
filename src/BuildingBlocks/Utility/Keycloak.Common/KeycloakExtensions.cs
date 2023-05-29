using Auth0.Abstract.Contracts;
using Keycloak.Common.Clients;
using Keycloak.Common.Interfaces;
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
            ArgumentNullException.ThrowIfNull(keycloackApplicationName);

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
        /// <param name="aurhoriazionServer">example: http://KEYCLOACKHOST/auth/realms/-InsertRealmNameHere-/</param>
        public static void UseKeycloakCredentialFlowService(this IServiceCollection services, string aurhoriazionServer)
        {
            ArgumentNullException.ThrowIfNull(aurhoriazionServer);

            services.AddOptions<KeycloakClientCredentialFlowOptions>().Configure(opt =>
            {
                opt.AuthorizationServerUrl = aurhoriazionServer;
            });

            services.AddScoped<IAuth0ClientCredentialFlowService, KeycloakCredentialFlowClient>();
        }
        /// <summary>
        /// Register keycloak client for oauth flows
        /// </summary>
        /// <param name="services"></param>
        /// <param name="aurhoriazionServer">example: http://KEYCLOACKHOST/auth/realms/-InsertRealmNameHere-/ -> usually it is master/</param>
        public static void UseKeycloakOwnerCredentialFlowService(this IServiceCollection services, string aurhoriazionServerOwner)
        {
            ArgumentNullException.ThrowIfNull(aurhoriazionServerOwner);

            //Resource owner password credentials flow
            services.AddOptions<KeycloakOwnerCredentialFlowOptions>().Configure(opt =>
            {
                opt.AuthorizationServerUrl = aurhoriazionServerOwner;
            });

            services.AddScoped<IAuth0OwnerCredentialFlowService, KeycloakOwnerCredentialFlowClient>();
        }

        /// <summary>
        /// Register keycloak client for userinfo endpoint
        /// </summary>
        /// <param name="services"></param>
        /// <param name="aurhoriazionServer"> example: http://KEYCLOACKHOST/auth/realms/-InsertRealmNameHere-/ </param>
        public static void UseKeycloakUserInfoService(this IServiceCollection services, string aurhoriazionServer)
        {
            ArgumentNullException.ThrowIfNull(aurhoriazionServer);

            //Client for userinfo endpoint
            services.AddOptions<KeycloakUserInfoOptions>().Configure(opt =>
            {
                opt.AuthorizationServerUrl = aurhoriazionServer;
            });

            services.AddHttpClient<IOpenIdUserInfoService, KeycloakUserInfoClient>();
        }

        /// <summary>
        /// Register keycloak client for admin api
        /// </summary>
        /// <param name="services"></param>
        /// <param name="aurhoriazionServer"></param>
        public static void UseKeycloakAdminService(this IServiceCollection services, string adminApiBase)
        {
            ArgumentNullException.ThrowIfNull(adminApiBase);

            services.AddOptions<KeycloakAdminApiOptions>().Configure(opt =>
            {
                opt.AdminApiEndpointBase = adminApiBase;
            });

            services.AddHttpClient<IKeycloakAdminService, KeycloakAdminClient>();
        }
    }
}
