using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography.Common.Utils
{
    /// <summary>
    /// Good article for RSA key formats 
    /// https://vcsjones.dev/key-formats-dotnet-3/
    /// </summary>
    public static class RsaUtils
    {
        /// <summary>
        /// “BEGIN PUBLIC KEY”
        /// </summary>
        /// <param name="publicKeyJwt">Public key</param>
        /// <returns></returns>
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

            var IssuerSigningKey = new RsaSecurityKey(rsa);

            return IssuerSigningKey;
        }
    }
}
