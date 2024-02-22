namespace Crypto.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    ICryptoRepository CryptoRepo { get; }
    ICryptoPriceRepository CryptoPriceRepo { get; }
    IVisitRepository VisitRepo { get; }
    ICryptoTimeFrameReadRepository TimeFrameRepo { get;  }
    Task Commit(CancellationToken ct = default);
}