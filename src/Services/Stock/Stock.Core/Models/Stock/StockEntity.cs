using Stock.Core.Models.Base;

namespace Stock.Core.Models.Stock
{
    public class StockEntity : AuditableEntity
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = default!;
        public ICollection<StockPriceEntity> Prices { get; set; } = new List<StockPriceEntity>();
    }
}
