using Crypto.Core.Entities;
using Crypto.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infrastracture.Persistence.Repositories
{
    public class VisitRepository : BaseRepository<Visit>, IVisitRepository
    {
        public VisitRepository(CryptoDbContext context) : base(context)
        {
        }
    }
}
