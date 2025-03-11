using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace WebProject.Common.Extensions
{
    public static class AuthorizationExtensions
    {
        public static void ConfigureDefaultAuthorization(this IServiceCollection services,
            string defaultSchemaName = JwtBearerDefaults.AuthenticationScheme)
        {
            services.AddAuthorization(opt =>
            {
                opt.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(defaultSchemaName)
                    .RequireAuthenticatedUser()
                    .Build();
                
                opt.AddPolicy("AdminPolicy", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(defaultSchemaName)
                    .RequireAuthenticatedUser()
                    .RequireRole("Admin")
                    .Build());
            });
        }
    }
}
