﻿using Cryptography.Common.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WebProject.Common.Options;

namespace WebProject.Common.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void ConfigureDefaultAuthentication(this IServiceCollection services, 
            AuthOptions options,
            Func<AuthenticationFailedContext, Task>? onTokenValidationFail = null,
            Func<TokenValidatedContext, Task>? onTokenValidated = null,
            Func<MessageReceivedContext, Task>? onMessageReceived = null)
        {
            ArgumentNullException.ThrowIfNull(nameof(services));
            ArgumentNullException.ThrowIfNull(nameof(options));

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
                    ValidateAudience = options.ValidateAudience,
                    ValidateIssuer = options.ValidateIssuer,
                    ValidIssuers = new[] { options.ValidIssuer },
                    ValidateIssuerSigningKey = options.ValidateIssuerSigningKey,
                    IssuerSigningKey = RsaUtils.ImportSubjectPublicKeyInfo(options.PublicRsaKey),
                    ValidateLifetime = options.ValidateLifetime,
                };

                opt.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = onTokenValidationFail ?? OnTokenValidationFailDefault,
                    OnTokenValidated = onTokenValidated ?? OnTokenValidatedDefault,
                    OnMessageReceived = onMessageReceived ?? OnMessageReceivedDefault,
                };
            });
        }

        private static Task OnTokenValidationFailDefault(AuthenticationFailedContext context)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
            logger?.LogTrace(context.Exception, "Exception occurred: {ExceptionMessage}", context.Exception.Message);
            return Task.CompletedTask;
        }

        private static Task OnTokenValidatedDefault(TokenValidatedContext context)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
            logger?.LogTrace("Token validated");
            return Task.CompletedTask;
        }
        
        private static Task OnMessageReceivedDefault(MessageReceivedContext context)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<JwtBearerEvents>>();
            logger?.LogTrace("Message received");
            return Task.CompletedTask;
        }
    }
}
