using Crypto.Core.Entities.PortfolioAggregate;
using Crypto.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infrastracture.Persistence.Repositories
{
    public class PortfolioRepository : IPortfolioRepositry
    {
        private readonly CryptoDbContext _context;

        public PortfolioRepository(CryptoDbContext context)
        {
            _context = context;
        }

        public async Task Add(Portfolio p)
        {
            await _context.AddAsync(p);
        }

        public async Task<Portfolio> GetPortfolio(string name, string userId)
        {
            //TODO add user ID
            return await _context.Portfolios.FirstOrDefaultAsync(i => i.Name == name);
        }

        public async Task<IEnumerable<Portfolio>> GetPortfolios(string userId)
        {
            //TODO add user ID
            return await _context.Portfolios.AsNoTracking().ToListAsync();
        }
    }
}
