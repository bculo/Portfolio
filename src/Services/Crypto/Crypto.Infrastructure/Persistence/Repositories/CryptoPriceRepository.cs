﻿using Crypto.Application.Interfaces.Repositories;
using Crypto.Core.Entities;
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

        public async Task<CryptoPrice?> GetLastPrice(string symbol, CancellationToken ct = default)
        {
            return await _context.Prices.Where(i => i.Crypto.Symbol.ToLower() == symbol.ToLower())
                .OrderByDescending(x => x.Time)
                .FirstOrDefaultAsync(ct);
        }

        public async Task BulkInsert(List<CryptoPrice> prices, CancellationToken ct = default)
        {
            await _context.BulkInsertAsync(prices, cancellationToken: ct);
        }
    }
}
