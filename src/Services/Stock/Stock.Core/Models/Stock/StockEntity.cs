using Stock.Core.Models.Base;

namespace Stock.Core.Models.Stock
{
    public class StockEntity : AuditableEntity
    {
        private StockEntity() {}
        public string Symbol { get; set; } = default!;
        public ICollection<StockPriceEntity> Prices { get; set; } = [];
        
        public static StockEntity Create(string symbol, decimal price)
        {
            var stockEntity = new StockEntity
            {
                Id = Guid.NewGuid(),
                Symbol = symbol,
                IsActive = true,
                Prices =
                [
                    new StockPriceEntity
                    {
                        Price = price
                    }
                ]
            };

            return stockEntity;
        }
    }
}
