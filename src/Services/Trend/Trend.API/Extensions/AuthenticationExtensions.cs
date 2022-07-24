using Cryptography.Common.Utils;
using Keycloak.Common;
using Keycloak.Common.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Trend.API.Services;
using Trend.Domain.Interfaces;

namespace Trend.API.Extensions
{
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Add JWT bearer authentication
        /// NOTE: Update System.IdentityModel.Tokens.Jwt to newest version to fix bux -> Method not found: 
        /// 'Void Microsoft.IdentityModel.Tokens.InternalValidators.ValidateLifetimeAndIssuerAfterSignatureNotValidatedJwt(Microsoft.IdentityModel.Tokens.SecurityToken, System.Nullable`1<System.DateTime>, System.Nullable`1<System.DateTime>, System.String, Microsoft.IdentityModel.Tokens.TokenValidationParameters, System.Text.StringBuilder)'.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            //Service for fetching relevant user data for this app
            services.AddScoped<ICurrentUser, UserService>();

            //Configure keycloak
            services.UseKeycloakClaimServices(configuration["KeycloakOptions:ApplicationName"]);
            services.UseKeycloakFlowService(configuration["KeycloakOptions:ApplicationName"]);

            //Define authentication using JWT token
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = configuration.GetValue<bool>("AuthOptions:ValidateAudience"),
                    ValidateIssuer = configuration.GetValue<bool>("AuthOptions:ValidateIssuer"),
                    ValidIssuers = new[] { configuration["AuthOptions:ValidIssuer"] },
                    ValidateIssuerSigningKey = configuration.GetValue<bool>("AuthOptions:ValidateIssuerSigningKey"),
                    IssuerSigningKey = RsaUtils.ImportSubjectPublicKeyInfo(configuration["AuthOptions:PublicRsaKey"]),
                    ValidateLifetime = configuration.GetValue<bool>("AuthOptions:ValidateLifetime")
                };

                //JWT events sections
                opt.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = c =>
                    {
                        Console.WriteLine("User successfully authenticated");
                        return Task.CompletedTask;
                    },
                };
            });
        }
    }
}
