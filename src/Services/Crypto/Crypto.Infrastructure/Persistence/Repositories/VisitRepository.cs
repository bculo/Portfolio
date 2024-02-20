using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Entities;

namespace Crypto.Infrastructure.Persistence.Repositories
{
    public class VisitRepository : BaseRepository<Visit>, IVisitRepository
    {
        public VisitRepository(CryptoDbContext context) : base(context)
        {
        }
    }
}
