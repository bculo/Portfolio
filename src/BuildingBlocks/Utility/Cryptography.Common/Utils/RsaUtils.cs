using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Cryptography.Common.Utils
{
    public static class RsaUtils
    {
        public static RsaSecurityKey ImportSubjectPublicKeyInfo(string publicKeyJwt)
        {
            if (string.IsNullOrEmpty(publicKeyJwt))
            {
                throw new ArgumentNullException(nameof(publicKeyJwt));
            }

            var rsa = RSA.Create();

            rsa.ImportSubjectPublicKeyInfo(
                source: Convert.FromBase64String(publicKeyJwt),
                bytesRead: out _
            );

            var issuerSigningKey = new RsaSecurityKey(rsa);

            return issuerSigningKey;
        }
    }
}
