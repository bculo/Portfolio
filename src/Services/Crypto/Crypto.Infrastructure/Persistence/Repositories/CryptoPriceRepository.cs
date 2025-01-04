using Crypto.Application.Interfaces.Repositories;
using Crypto.Application.Interfaces.Repositories.Models;
using Crypto.Core.Entities;
using Crypto.Core.ReadModels;
using Crypto.Infrastructure.Persistence.Extensions;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Infrastructure.Persistence.Repositories
{
    public class CryptoPriceRepository(CryptoDbContext context) : ICryptoPriceRepository
    {
        public async Task Add(CryptoPriceEntity priceEntity, CancellationToken ct = default)
        {
            await context.Database.ExecuteSqlInterpolatedAsync(
                $"INSERT INTO public.crypto_price(\"time\", price, cryptoentityid) VALUES ({priceEntity.Time}, {priceEntity.Price}, {priceEntity.CryptoEntityId});",
                ct);
        }
        
        public async Task BulkInsert(List<CryptoPriceEntity> prices, CancellationToken ct = default)
        {
            await context.BulkInsertAsync(prices, cancellationToken: ct);
        }

        public async Task<CryptoLastPriceReadModel?> GetLastPrice(Guid id, CancellationToken ct = default)
        {
            return await context.Prices.Where(i => i.CryptoEntityId == id)
                .Include(x => x.CryptoEntity)
                .OrderByDescending(x => x.Time)
                .Select(i => new CryptoLastPriceReadModel
                {
                    CryptoId = i.CryptoEntityId,
                    Name = i.CryptoEntity.Name,
                    Symbol = i.CryptoEntity.Symbol,
                    Website = i.CryptoEntity.WebSite,
                    LastPrice = i.Price,
                    SourceCode = i.CryptoEntity.SourceCode
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<CryptoLastPriceReadModel?> GetLastPrice(string symbol, CancellationToken ct = default)
        {
            return await context.Prices.Where(i => i.CryptoEntity.Symbol.ToLower() == symbol.ToLower())
                .Include(x => x.CryptoEntity)
                .OrderByDescending(x => x.Time)
                .Select(i => new CryptoLastPriceReadModel
                {
                    CryptoId = i.CryptoEntityId,
                    Name = i.CryptoEntity.Name,
                    Symbol = i.CryptoEntity.Symbol,
                    Website = i.CryptoEntity.WebSite,
                    LastPrice = i.Price,
                    SourceCode = i.CryptoEntity.SourceCode
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<PageResult<CryptoLastPriceReadModel>> GetPage(CryptoPricePageQuery req, CancellationToken ct = default)
        {
            var query = context.CryptoLastPrice.AsQueryable();

            if (!string.IsNullOrWhiteSpace(req.Symbol))
            {
                query = query.Where(x => EF.Functions.ILike(x.Symbol, req.Symbol.ToContainsPattern()));
            }

            var count = await query.CountAsync(ct);
            var items = await query.OrderBy(x => x.Symbol)
                .Skip(req.Skip)
                .Take(req.Take)
                .ToListAsync(ct);

            return new PageResult<CryptoLastPriceReadModel>(count, req.Page, items);
        }

        private IQueryable<CryptoTimeFrameReadModel> GetQuery(int notOlderThanMin, int timeBucketMin)
        {
            return context.CryptoTimeFrame(notOlderThanMin, timeBucketMin);
        }

        public async Task<List<CryptoTimeFrameReadModel>> GetTimeFrameData(TimeFrameQuery query, 
            CancellationToken ct = default)
        {
            return await GetQuery(query.NotOlderThanMin, query.TimeBucketMin).ToListAsync(ct);
        }

        public async Task<List<CryptoTimeFrameReadModel>> GetTimeFrameData(Guid cryptoId, 
            TimeFrameQuery query, 
            CancellationToken ct = default)
        {
            return await GetQuery(query.NotOlderThanMin, query.TimeBucketMin)
                .Where(i => i.CryptoId == cryptoId)
                .ToListAsync(ct);
        }
    }
}
