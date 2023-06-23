using Cryptography.Common.Utils;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using User.Functions.Options;

namespace User.Functions.Services
{
    public interface ITokenService
    {
        Task<TokenServiceResult> Validate(string token);
    }

    public class TokenServiceResult
    {
        public IEnumerable<Claim> Claims { get; private set; }
        public bool IsValid { get; private set; }
        public string FailureReason { get; set; }

        protected TokenServiceResult(IEnumerable<Claim> claims, bool isValid, string reasonForFailure)
        {
            Claims = claims;
            IsValid = isValid;
            FailureReason = reasonForFailure;
        }

        public static TokenServiceResult Failure(string message)
        {
            return new TokenServiceResult(Enumerable.Empty<Claim>(), false, message);
        }

        public static TokenServiceResult Success(IEnumerable<Claim> claims)
        {
            return new TokenServiceResult(claims, true, null);
        }
    }

    public class JwtTokenService : ITokenService
    {
        private readonly JwtValidationOptions _options;
        private readonly ILogger<JwtTokenService> _logger;

        public JwtTokenService(ILogger<JwtTokenService> logger, IOptions<JwtValidationOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public async Task<TokenServiceResult> Validate(string token)
        {
            ArgumentException.ThrowIfNullOrEmpty(token, nameof(token));

            var tokenHandler = new JwtSecurityTokenHandler();
            var result = await tokenHandler.ValidateTokenAsync(token, 
                new TokenValidationParameters
                {
                    ValidateAudience = _options.ValidateAudience,
                    ValidateIssuer = _options.ValidateIssuer,
                    ValidIssuers = new[] { _options.ValidIssuer },
                    ValidateIssuerSigningKey = _options.ValidateIssuerSigningKey,
                    IssuerSigningKey = RsaUtils.ImportSubjectPublicKeyInfo(_options.PublicRsaKey),
                    ValidateLifetime = _options.ValidateLifetime,
                });

            if (!result.IsValid)
            {
                _logger.LogWarning(result.Exception.Message);
                var userExceptionMessage = GetUserExceptionMessage(result.Exception.Message);
                return TokenServiceResult.Failure(userExceptionMessage);
            }

            return TokenServiceResult.Success(result.ClaimsIdentity.Claims);
        }

        private string GetUserExceptionMessage(string message)
        {
            if(message.Contains("Lifetime validation failed", StringComparison.CurrentCultureIgnoreCase))
            {
                return "Authorization token expired";
            }

            return "Authorization token is not valid";
        }
    }
}
