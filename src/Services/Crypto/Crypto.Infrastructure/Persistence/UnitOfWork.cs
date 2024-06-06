using Crypto.Application.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Crypto.Infrastructure.Persistence
{
    public class UnitOfWork(CryptoDbContext context, IServiceProvider provider) : IUnitOfWork
    {
        public ICryptoRepository CryptoRepo { get; } = provider.GetRequiredService<ICryptoRepository>();
        public ICryptoPriceRepository CryptoPriceRepo { get; } = provider.GetRequiredService<ICryptoPriceRepository>();
        public IVisitRepository VisitRepo { get; } = provider.GetRequiredService<IVisitRepository>();


        public async Task Commit(CancellationToken ct = default)
        {
            await context.SaveChangesAsync(ct);
        }
    }
}
