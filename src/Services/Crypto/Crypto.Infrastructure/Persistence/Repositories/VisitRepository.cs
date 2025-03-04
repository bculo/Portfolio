﻿using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Entities;
using Crypto.Core.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Infrastructure.Persistence.Repositories
{
    public class VisitRepository : BaseRepository<VisitEntity>, IVisitRepository
    {
        public VisitRepository(CryptoDbContext context) : base(context)
        {
        }

        public async Task<List<MostPopularReadModel>> GetMostPopular(int take, CancellationToken ct = default)
        {
            return await Set
                .Include(x => x.Crypto)
                .GroupBy(i => i.Crypto!.Symbol)
                .OrderByDescending(i => i.Count())
                .Take(take)
                .Select(i => new MostPopularReadModel
                {
                    Symbol = i.Key,
                    Count = i.Count(),
                })
                .ToListAsync(ct);
        }
    }
}
