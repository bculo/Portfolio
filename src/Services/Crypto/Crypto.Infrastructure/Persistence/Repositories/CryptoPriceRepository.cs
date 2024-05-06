using Crypto.Application.Interfaces.Repositories;
using Crypto.Application.Interfaces.Repositories.Models;
using Crypto.Core.Entities;
using Crypto.Core.ReadModels;
using Crypto.Infrastructure.Persistence.Extensions;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace Crypto.Infrastructure.Persistence.Repositories
{
    public class CryptoPriceRepository : ICryptoPriceRepository
    {
        private readonly CryptoDbContext _context;
        
        public CryptoPriceRepository(CryptoDbContext context)
        {
            _context = context;
        }

        public async Task Add(CryptoPrice price, CancellationToken ct = default)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"INSERT INTO public.crypto_price(\"time\", price, cryptoid) VALUES ({price.Time}, {price.Price}, {price.CryptoId});",
                ct);
        }
        
        public async Task BulkInsert(List<CryptoPrice> prices, CancellationToken ct = default)
        {
            await _context.BulkInsertAsync(prices, cancellationToken: ct);
        }

        public async Task<CryptoLastPriceReadModel?> GetLastPrice(Guid id, CancellationToken ct = default)
        {
            return await _context.Prices.Where(i => i.CryptoId == id)
                .Include(x => x.Crypto)
                .OrderByDescending(x => x.Time)
                .Select(i => new CryptoLastPriceReadModel
                {
                    CryptoId = i.CryptoId,
                    Name = i.Crypto.Name,
                    Symbol = i.Crypto.Symbol,
                    Website = i.Crypto.WebSite,
                    LastPrice = i.Price,
                    SourceCode = i.Crypto.SourceCode
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<CryptoLastPriceReadModel?> GetLastPrice(string symbol, CancellationToken ct = default)
        {
            return await _context.Prices.Where(i => i.Crypto.Symbol.ToLower() == symbol.ToLower())
                .Include(x => x.Crypto)
                .OrderByDescending(x => x.Time)
                .Select(i => new CryptoLastPriceReadModel
                {
                    CryptoId = i.CryptoId,
                    Name = i.Crypto.Name,
                    Symbol = i.Crypto.Symbol,
                    Website = i.Crypto.WebSite,
                    LastPrice = i.Price,
                    SourceCode = i.Crypto.SourceCode
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<List<CryptoLastPriceReadModel>> GetPage(CryptoPricePageQuery req, CancellationToken ct = default)
        {
            var query = _context.CryptoLastPrice.AsQueryable();

            if (!string.IsNullOrWhiteSpace(req.Symbol))
            {
                query = query.Where(x => EF.Functions.ILike(x.Symbol, req.Symbol.ToContainsPattern()));
            }
            
            return await query.OrderBy(x => x.Symbol)
                .Skip(req.Skip)
                .Take(req.Take)
                .ToListAsync(ct);
        }

        private IQueryable<CryptoTimeFrameReadModel> GetQuery(int notOlderThanMin, int timeBucketMin)
        {
            return _context.CryptoTimeFrame(notOlderThanMin, timeBucketMin);
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
