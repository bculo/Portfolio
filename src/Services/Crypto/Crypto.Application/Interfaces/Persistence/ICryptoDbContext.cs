using Microsoft.EntityFrameworkCore;

namespace Crypto.Application.Interfaces.Persistence
{
    public interface ICryptoDbContext
    {
        public DbSet<Core.Entities.Crypto> Cryptos { get; }
        public DbSet<Core.Entities.CryptoPrice> Prices { get; }
        public DbSet<Core.Entities.Visit> Visits { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
