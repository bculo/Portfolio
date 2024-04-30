namespace Events.Common.Stock;

public class StockActivated
{
    public string Symbol { get; set; } = default!;
    public DateTime Time { get; set; }
}