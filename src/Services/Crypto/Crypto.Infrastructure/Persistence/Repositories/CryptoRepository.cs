using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Entities;
using Dapper;

namespace Crypto.Infrastructure.Persistence.Repositories
{
    public class CryptoRepository : BaseRepository<Core.Entities.Crypto>, ICryptoRepository
    {
        public CryptoRepository(CryptoDbContext context) : base(context)
        {

        }
    }
}
