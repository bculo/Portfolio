using Crypto.Core.Interfaces;
using Crypto.Infrastracture.Persistence.Repositories;
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

        public UnitOfWork(CryptoDbContext context)
        {
            _context = context;

            CryptoPriceRepository = new CryptoPriceRepository(context);
            CryptoRepository = new CryptoRepository(context);
            CryptoExplorerRepository = new CryptoExplorerRepository(context);
        }

        public virtual async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
