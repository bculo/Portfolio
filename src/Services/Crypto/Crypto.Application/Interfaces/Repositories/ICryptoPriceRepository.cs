﻿using Crypto.Application.Interfaces.Repositories.Models;
using Crypto.Core.Entities;
using Crypto.Core.ReadModels;

namespace Crypto.Application.Interfaces.Repositories
{
    public interface ICryptoPriceRepository
    {
        Task Add(CryptoPriceEntity priceEntity, CancellationToken ct = default);
        Task BulkInsert(List<CryptoPriceEntity> prices, CancellationToken ct = default);
        Task<CryptoLastPriceReadModel?> GetLastPrice(Guid id, CancellationToken ct = default);
        Task<CryptoLastPriceReadModel?> GetLastPrice(string symbol, CancellationToken ct = default);
        Task<PageResult<CryptoLastPriceReadModel>> GetPage(CryptoPricePageRepoQuery repoQuery, CancellationToken ct = default);
        Task<List<CryptoTimeFrameReadModel>> GetTimeFrameData(TimeFrameQuery query, CancellationToken ct = default);
        Task<List<CryptoTimeFrameReadModel>> GetTimeFrameData(Guid cryptoId, 
            TimeFrameQuery query, 
            CancellationToken ct = default);
    }
}
