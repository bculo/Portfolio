using Crypto.Core.Entities;
using Crypto.Core.ReadModels;

namespace Crypto.Application.Interfaces.Repositories
{
    public interface IVisitRepository : IRepository<VisitEntity>
    {
        Task<List<MostPopularReadModel>> GetMostPopular(int take, CancellationToken ct = default);
    }
}
