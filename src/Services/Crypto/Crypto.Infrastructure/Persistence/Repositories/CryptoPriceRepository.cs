using Crypto.Core.Entities;
using Crypto.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infrastructure.Persistence.Repositories
{
    public class CryptoPriceRepository : ICryptoPriceRepository
    {
        public CryptoPriceRepository(CryptoDbContext context)
        {

        }
    }
}
