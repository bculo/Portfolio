using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace WebProject.Common.Extensions
{
    public static class AuthorizationExtensions
    {
        public static void ConfigureDefaultAuthorization(this IServiceCollection services, 
            string policyName = "BearerPolicy")
        {
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(policyName, new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
                
                opt.DefaultPolicy = opt.GetPolicy(policyName) ?? throw new ArgumentNullException(nameof(policyName));
            });
        }
    }
}
