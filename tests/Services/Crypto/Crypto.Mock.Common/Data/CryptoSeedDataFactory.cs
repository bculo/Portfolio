using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Mock.Common.Data
{
    public static class CryptoSeedDataFactory
    {
        public static ICryptoDataManager GetDataSeeder()
        {
            return new CryptoDataManager();
        }
    }
}
