using Stock.Application.Interfaces.Repositories;
using Stock.Core.Models.Stock;

namespace Stock.Infrastructure.Persistence.Repositories;

public class StockPriceRepository(StockDbContext dbContext)
    : BaseRepository<StockPriceEntity>(dbContext), IStockPriceRepository;