using Stock.Core.Models.Base;

namespace Stock.Core.Models.Stock
{
    public class StockWithPriceTag : IReadModel
    {
        public Guid StockId { get; set; }
        public string Symbol { get; set; } = default!;
        public decimal? Price { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? LastPriceUpdate { get; set; }
    }
}
