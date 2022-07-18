using Keycloak.Common.Options;
using Keycloak.Common.Services;
using Microsoft.AspNetCore.Authentication;
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
        public static void AddKeyCloakClaimTransormer(this IServiceCollection services)
        {
            services.AddTransient<IClaimsTransformation, KeycloakClaimsTransformer>();
        }
    }
}
