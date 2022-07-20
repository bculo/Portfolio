using Keycloak.Common.Interfaces;
using Keycloak.Common.Options;
using Keycloak.Common.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keycloak.Common
{
    public static class KeycloakExtensions
    {
        public static void UseKeycloak(this IServiceCollection services, IConfiguration configuration, string keycloackConfigSection = "KeycloakOptions")
        {
            //Configure KeycloakClaimsTransformer
            services.Configure<KeycloakOptions>(configuration.GetSection(keycloackConfigSection));
            services.AddTransient<IClaimsTransformation, KeycloakClaimsTransformer>();

            services.AddHttpContextAccessor();
            services.AddScoped<IKeycloakUser, KeycloackUserInfo>();
        }
    }
}
