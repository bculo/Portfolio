using Crypto.Core.Interfaces;
using Crypto.Infrastracture.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infrastracture.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CryptoDbContext _context;

        public ICryptoPriceRepository CryptoPriceRepository { get; private set; }
        public ICryptoRepository CryptoRepository { get; private set; }
        public ICryptoExplorerRepository CryptoExplorerRepository { get; private set; }
        public IVisitRepository VisitRepository { get; private set; }
        public IPortfolioRepositry PortfolioRepositry { get; private set; }

        public UnitOfWork(CryptoDbContext context, IServiceProvider provider)
        {
            _context = context;

            CryptoPriceRepository = provider.GetService<ICryptoPriceRepository>() ?? throw new ArgumentNullException();
            CryptoRepository = provider.GetService<ICryptoRepository>() ?? throw new ArgumentNullException();
            CryptoExplorerRepository = provider.GetService<ICryptoExplorerRepository>() ?? throw new ArgumentNullException();
            VisitRepository = provider.GetService<IVisitRepository>() ?? throw new ArgumentNullException();
            PortfolioRepositry = provider.GetService<IPortfolioRepositry>() ?? throw new ArgumentNullException();
        }

        public virtual async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
