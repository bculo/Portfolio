using Crypto.Application.Interfaces.Repositories;
using Crypto.Application.Interfaces.Repositories.Models;
using Crypto.Core.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Infrastructure.Persistence.Repositories.Functions;

public class CryptoTimeFrameReadRepository : ICryptoTimeFrameReadRepository
{
    private readonly CryptoDbContext _context;
    
    public CryptoTimeFrameReadRepository(CryptoDbContext context)
    {
        _context = context;
    }

    private IQueryable<CryptoTimeFrameReadModel> GetQuery(int notOlderThanMin, int timeBucketMin)
    {
        return _context.CryptoWithPrices(notOlderThanMin, timeBucketMin);
    }

    public async Task<List<CryptoTimeFrameReadModel>> GetAll(TimeFrameQuery query, 
        CancellationToken ct = default)
    {
        return await GetQuery(query.NotOlderThanMin, query.TimeBucketMin).ToListAsync(ct);
    }

    public async Task<CryptoTimeFrameReadModel?> GetSingle(Guid cryptoId, 
        TimeFrameQuery query, 
        CancellationToken ct = default)
    {
        return await GetQuery(query.NotOlderThanMin, query.TimeBucketMin)
            .Where(i => i.CryptoId == cryptoId)
            .FirstOrDefaultAsync(ct);
    }
}