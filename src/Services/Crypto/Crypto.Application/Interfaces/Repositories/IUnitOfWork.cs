namespace Crypto.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    ICryptoRepository CryptoRepo { get; }
    ICryptoPriceRepository CryptoPriceRepo { get; }
    IVisitRepository VisitRepo { get; }
    Task Commit(CancellationToken ct = default);
}