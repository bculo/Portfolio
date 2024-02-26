using Crypto.Core.Entities;
using Crypto.Core.ReadModels;

namespace Crypto.Application.Interfaces.Repositories
{
    public interface IVisitRepository : IRepository<Visit>
    {
        Task<List<MostPopular>> GetMostPopular(int take, CancellationToken ct = default);
    }
}
