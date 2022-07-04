using Crypto.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infrastracture.Persistence.Repositories
{
    public class CryptoRepository : BaseRepository<Core.Entities.Crypto>, ICryptoRepository
    {
        public CryptoRepository(CryptoDbContext context) : base(context)
        {

        }
    }
}
