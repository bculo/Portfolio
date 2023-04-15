using Cryptography.Common.Utils;
using Keycloak.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Trend.API.Services;
using Trend.Domain.Interfaces;

namespace Trend.API.Extensions
{
    public static class ServiceConfigurationExtensions
    {
        public static void ConfigureApiProject(this IServiceCollection services, IConfiguration configuration)
        {

        }

        private static void ConfigureAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            //Service for fetching relevant user data for this app
            services.AddScoped<ICurrentUser, UserService>();

            //Configure keycloak
            services.UseKeycloakClaimServices(configuration["KeycloakOptions:ApplicationName"]);
            services.UseKeycloakCredentialFlowService(configuration["KeycloakOptions:AuthorizationServerUrl"]);

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

                    OnAuthenticationFailed = c =>
                    {
                        Console.WriteLine("Problem with authentication");
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddSwaggerGen(opt =>
            {
                var authorizationUrl = $"{configuration["KeycloakOptions:AuthorizationServerUrl"]}/protocol/openid-connect/auth";

                opt.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(authorizationUrl),
                        }
                    },
                    In = ParameterLocation.Header,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }
    }
}
