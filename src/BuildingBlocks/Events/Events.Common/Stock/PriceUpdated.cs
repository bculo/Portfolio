namespace Events.Common.Stock
{
    public class PriceUpdated
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
