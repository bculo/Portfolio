namespace Events.Common.Stock
{
    public class NewStockItemAdded
    {
        public string Symbol { get; set; } = default!;
        public decimal Price { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
