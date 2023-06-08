namespace Events.Common.Stock
{
    public class NewStockItemAdded
    {
        public string Symbol { get; set; }
        public DateTime Created { get; set; }
    }
}
