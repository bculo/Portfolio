namespace Events.Common.Stock
{
    public class StockPriceUpdated
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
