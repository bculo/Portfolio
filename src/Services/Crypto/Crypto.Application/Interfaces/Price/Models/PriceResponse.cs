namespace Crypto.Application.Interfaces.Price.Models;

public class PriceResponse
{
    public string Symbol { get; set; } = default!;
    public string Currency { get; set; } = default!;
    public decimal Price { get; set; }
}