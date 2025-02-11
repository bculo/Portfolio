using Stock.Core.Models.Base;

namespace Stock.Core.Models.Stock
{
    public class StockEntity : AuditableEntity
    {
        private StockEntity() {}
        
        public string Symbol { get; set; } = default!;
        public ICollection<StockPriceEntity> Prices { get; set; } = [];

        public static StockEntity New(string symbol)
        {
            return new StockEntity
            {
                Id = Guid.NewGuid(),
                Symbol = symbol,
                IsActive = true
            };
        }
        
        public static StockEntity NewWithPrice(string symbol, decimal price)
        {
            var stockEntity = New(symbol);
            stockEntity.Prices =
            [
                new StockPriceEntity
                {
                    Price = price
                }
            ];
            
            return stockEntity;
        }
    }
}
