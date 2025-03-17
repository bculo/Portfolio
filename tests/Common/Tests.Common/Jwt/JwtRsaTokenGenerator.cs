using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Tests.Common.Jwt;

public interface ITokenGenerator
{
    Task<string> GenerateToken(List<Claim> claims);
}

public class JwtRsaTokenGenerator(IConfiguration config) : ITokenGenerator, IDisposable
{
    private readonly RSA _rsa = RSA.Create();
    
    public Task<string> GenerateToken(List<Claim> claims)
    {
        _rsa.ImportPkcs8PrivateKey(GetRsaPrivateKey(), out _);

        var credentials = new SigningCredentials(new RsaSecurityKey(_rsa), SecurityAlgorithms.RsaSha256);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = credentials,
            Issuer = config.GetValue<string?>("AuthOptions:ValidIssuer") ?? throw new Exception("Valid issuer not found")
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return Task.FromResult(jwt);
    }

    private byte[] GetRsaPrivateKey()
    {
        var privateKey = config.GetValue<string?>("TestAuthentication:PrivateKeyBase64") ??
            throw new Exception("RSA private key not found");
        
        return Convert.FromBase64String(privateKey);
    }

    public void Dispose()
    {
        _rsa.Dispose();
    }
}