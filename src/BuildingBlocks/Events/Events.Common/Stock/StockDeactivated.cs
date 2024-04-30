namespace Events.Common.Stock;

public class StockDeactivated
{
    public string Symbol { get; set; } = default!;
    public DateTime Time { get; set; }
}