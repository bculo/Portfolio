using Crypto.Core.Interfaces;
using Crypto.Core.Queries.Response;
using Microsoft.EntityFrameworkCore;
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

        public async Task<CryptoResponseQuery> GetWithPrice(string symbol)
        {
            return await _context.Prices.Include(i => i.Crypto)
                                    .Where(i => i.Crypto.Symbol.ToLower() == symbol.ToLower())
                                    .OrderByDescending(i => i.CreatedOn)
                                    .Select(i => new CryptoResponseQuery
                                    {
                                        Created = i.Crypto.CreatedOn,
                                        Name = i.Crypto.Name,
                                        Symbol = i.Crypto.Symbol,
                                        Description = i.Crypto.Description,
                                        Logo = i.Crypto.Logo,
                                        SourceCode = i.Crypto!.SourceCode,
                                        Website = i.Crypto!.WebSite,
                                        Price = i.Price
                                    }).FirstOrDefaultAsync();                               
        }
    }
}
