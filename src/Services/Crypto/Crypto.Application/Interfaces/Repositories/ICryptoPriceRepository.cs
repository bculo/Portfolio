using Crypto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto.Application.Interfaces.Repositories
{
    public interface ICryptoPriceRepository
    {
        Task Add(CryptoPrice price, CancellationToken ct = default);
        Task<CryptoPrice?> GetLastPrice(Guid id, CancellationToken ct = default);
        Task<CryptoPrice?> GetLastPrice(string symbol, CancellationToken ct = default);
        Task BulkInsert(List<CryptoPrice> prices, CancellationToken ct = default);
    }
}
