using Crypto.Core.Interfaces;
using Crypto.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CryptoDbContext _context;
        
        public ICryptoRepository CryptoRepository { get; private set; } 
        public IVisitRepository VisitRepository { get; private set; }

        public UnitOfWork(CryptoDbContext context, IServiceProvider provider)
        {
            _context = context;
            
            CryptoRepository = provider.GetService<ICryptoRepository>() ?? throw new ArgumentNullException();
            VisitRepository = provider.GetService<IVisitRepository>() ?? throw new ArgumentNullException();
        }

        public virtual async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
