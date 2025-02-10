namespace Stock.Application.Interfaces.Price.Models;

public class StockPriceInfo
{
    public string Symbol { get; set; }  = null!;
    public decimal Price { get; set; }
    public DateTime FetchedTimestamp { get; set; }
}