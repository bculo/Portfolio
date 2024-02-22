using Crypto.Application.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Crypto.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CryptoDbContext _context;
        
        public ICryptoRepository CryptoRepo { get; }
        public ICryptoPriceRepository CryptoPriceRepo { get; }
        public IVisitRepository VisitRepo { get; }
        
        public ICryptoTimeFrameReadRepository TimeFrameRepo { get; }

        public UnitOfWork(CryptoDbContext context, IServiceProvider provider)
        {
            _context = context;
            
            CryptoRepo = provider.GetRequiredService<ICryptoRepository>();
            CryptoPriceRepo = provider.GetRequiredService<ICryptoPriceRepository>();
            VisitRepo = provider.GetRequiredService<IVisitRepository>();
            TimeFrameRepo = provider.GetRequiredService<ICryptoTimeFrameReadRepository>();
        }

        public async Task Commit(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}
