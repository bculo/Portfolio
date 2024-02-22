using Crypto.Application.Interfaces.Repositories.Models;
using Crypto.Core.ReadModels;

namespace Crypto.Application.Interfaces.Repositories;

public interface ICryptoTimeFrameReadRepository
{
    Task<List<CryptoTimeFrameReadModel>> GetAll(TimeFrameQuery query, CancellationToken ct = default);
    Task<CryptoTimeFrameReadModel?> GetSingle(Guid cryptoId, TimeFrameQuery query, CancellationToken ct = default);
}