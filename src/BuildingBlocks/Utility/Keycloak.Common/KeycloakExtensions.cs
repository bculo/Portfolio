﻿using Ardalis.GuardClauses;
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
        /// <param name="aurhoriazionServer"></param>
        public static void UseKeycloakFlowService(this IServiceCollection services, string aurhoriazionServer)
        {
            Guard.Against.NullOrEmpty(aurhoriazionServer);

            //Client credentials flow
            services.AddOptions<KeycloakClientCredentialFlowOptions>().Configure(opt =>
            {
                opt.AuthorizationServerUrl = aurhoriazionServer;
            });
            services.AddHttpClient<IAuth0ClientCredentialFlowService, KeycloakCredentialFlowClient>();

            //Resource owner password credentials flow
            services.AddOptions<KeycloakOwnerCredentialFlowOptions>().Configure(opt =>
            {
                opt.AuthorizationServerUrl = aurhoriazionServer;
            });
            services.AddHttpClient<IAuth0OwnerCredentialFlowService, KeycloakOwnerCredentialFlowClient>();
        }

        /// <summary>
        /// Register keycloak client for userinfo endpoint
        /// </summary>
        /// <param name="services"></param>
        /// <param name="aurhoriazionServer"></param>
        public static void UseKeycloakUserInfoService(this IServiceCollection services, string aurhoriazionServer)
        {
            Guard.Against.NullOrEmpty(aurhoriazionServer);

            //Client for userinfo endpoint
            services.AddOptions<KeycloakUserInfoOptions>().Configure(opt =>
            {
                opt.AuthorizationServerUrl = aurhoriazionServer;
            });
            services.AddHttpClient<IOpenIdUserInfoService, KeycloakUserInfoClient>();
        }
    }
}
