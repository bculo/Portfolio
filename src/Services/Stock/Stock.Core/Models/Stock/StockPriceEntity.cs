using Stock.Core.Models.Base;

namespace Stock.Core.Models.Stock
{
    public class StockPriceEntity : AuditableEntity
    {
        public decimal Price { get; set; }
        public int StockId { get; set; }
        public StockEntity? StockEntity { get; set; }
    }
}
