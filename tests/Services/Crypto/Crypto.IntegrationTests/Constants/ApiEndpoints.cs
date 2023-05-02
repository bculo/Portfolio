using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.IntegrationTests.Constants
{
    public static class ApiEndpoint
    {
        public const string INFO_VERSION = "/api/v1/Info/AssemblyVersion";

        public const string CRYPTO_FETCH_ALL = "/api/v1/crypto/fetchall";
        public const string CRYPTO_FETCH_PAGE = "/api/v1/crypto/fetchpage";
        public const string CRYPTO_FETCH_SINGLE = "/api/v1/crypto/single";
        public const string CRYPTO_PRICE_HISTORY = "/api/v1/crypto/GetPriceHisotry";
        public const string CRYPTO_MOST_POPULAR = "/api/v1/crypto/GetMostPopular";
        public const string CRYPTO_DELETE = "/api/v1/crypto/delete";
        public const string CRYPTO_ADD_NEW = "/api/v1/crypto/AddNew";
        public const string CRYPTO_ADD_NEW_DELAY = "/api/v1/crypto/AddNewWithDelay";
        public const string CRYPTO_ADD_UNDO_DELAY = "/api/v1/crypto/UndoAddNewDelay";
        public const string CRYPTO_UPDATE_PRICE = "/api/v1/crypto/UpdatePrice";
        public const string CRYPTO_UPDATE_INFO = "/api/v1/crypto/UpdateInfo";
        public const string CRYPTO_UPDATE_PRICE_ALL = "/api/v1/crypto/UpdateAllPrices";
    }
}
