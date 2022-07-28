using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.Constants
{
    public static class ApiEndpoint
    {
        public const string INFO_VERSION = "/api/info";

        public const string CRYPTO_FETCH_ALL = "/api/crypto/fetchall";
        public const string CRYPTO_FETCH_SINGLE = "/api/crypto/single";
        public const string CRYPTO_PRICE_HISTORY = "/api/crypto/GetPriceHisotry";
        public const string CRYPTO_DELETE = "/api/crypto/delete";
    }
}
