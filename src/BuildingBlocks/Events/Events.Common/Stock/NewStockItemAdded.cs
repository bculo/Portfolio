namespace Events.Common.Stock
{
    public class NewStockItemAdded
    {
        public string Symbol { get; set; } = default!;
        public DateTime Created { get; set; }
    }
}
