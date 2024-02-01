namespace Stock.Application.Common.Models;

public record StockPriceResultDto
{
    public string Id { get; init; } = default!;
    public string Symbol { get; init; } = default!;
    public decimal? Price { get; init; }
    public bool IsActive { get; init; }
    public DateTime? LastPriceUpdate { get; init; }
    public DateTime Created { get; init; }
}