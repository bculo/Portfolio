using Cryptography.Common.Utils;
using Keycloak.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Notification.Hub.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.UseKeycloakClaimServices(configuration["KeycloakOptions:ApplicationName"]);

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

                opt.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("User successfully authenticated");
                        return Task.CompletedTask;
                    },

                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/portfolio")))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },

                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("Failed authentication");
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
