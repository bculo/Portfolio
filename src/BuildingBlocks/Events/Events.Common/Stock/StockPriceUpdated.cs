namespace Events.Common.Stock
{
    public class StockPriceUpdated
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = default!;
        public decimal Price { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
