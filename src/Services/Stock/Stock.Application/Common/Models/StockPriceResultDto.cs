namespace Stock.Application.Common.Models;

public record StockPriceResultDto
{
    public required Guid Id { get; init; } 
    public required string Symbol { get; init; } = default!;
    public decimal? Price { get; init; }
    public required bool IsActive { get; init; }
    public DateTimeOffset? LastPriceUpdate { get; init; }
    public DateTimeOffset Created { get; init; }
}