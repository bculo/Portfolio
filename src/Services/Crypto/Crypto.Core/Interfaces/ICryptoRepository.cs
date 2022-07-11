using Crypto.Core.Queries.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Core.Interfaces
{
    public interface ICryptoRepository : IRepository<Core.Entities.Crypto>
    {
        Task<CryptoResponseQuery> GetWithPrice(string symbol);
        Task<List<CryptoResponseQuery>> GetAllWithPrice();
    }
}
