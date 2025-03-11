using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Cryptography.Common;

public record RsaKeyPair(string PrivateKey, string PublicKey);

public static class RsaUtils
{
    public static RsaSecurityKey ImportSubjectPublicKeyInfo(string publicKeyJwt)
    {
        if (string.IsNullOrEmpty(publicKeyJwt))
        {
            throw new ArgumentNullException(nameof(publicKeyJwt));
        }

        using var rsa = RSA.Create();
        
        rsa.ImportSubjectPublicKeyInfo(
            source: Convert.FromBase64String(publicKeyJwt),
            bytesRead: out _
        );

        var issuerSigningKey = new RsaSecurityKey(rsa.ExportParameters(false));

        return issuerSigningKey;
    }

    public static RsaKeyPair GenerateKeyPair(int keySize = 2048)
    {
        using var rsa = RSA.Create(keySize);
        var privateKey = Convert.ToBase64String(rsa.ExportPkcs8PrivateKey());
        var publicKey = Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo());
        return new RsaKeyPair(privateKey, publicKey);
    }
}

