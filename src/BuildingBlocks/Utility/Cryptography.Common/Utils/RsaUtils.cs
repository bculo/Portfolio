using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

            RSA rsa = RSA.Create();

            rsa.ImportSubjectPublicKeyInfo(
                source: Convert.FromBase64String(publicKeyJwt),
                bytesRead: out _
            );

            var issuerSigningKey = new RsaSecurityKey(rsa);

            return issuerSigningKey;
        }
    }
}
