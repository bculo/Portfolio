using Crypto.Core.Entities;
using Crypto.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infrastracture.Persistence.Repositories
{
    public class CryptoPriceRepository : BaseRepository<CryptoPrice>, ICryptoPriceRepository
    {
        public CryptoPriceRepository(CryptoDbContext context) : base(context)
        {

        }
    }
}
