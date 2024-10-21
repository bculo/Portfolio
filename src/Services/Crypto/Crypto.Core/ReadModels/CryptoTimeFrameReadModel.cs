namespace Crypto.Core.ReadModels;

public class CryptoTimeFrameReadModel
{
    public Guid CryptoId { get; init; }
    public string Symbol { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Website { get; init; } = default!;
    public string SourceCode { get; init; } = default!;
    public DateTimeOffset TimeBucket { get; init; }
    public decimal AvgPrice { get; init; }
    public decimal MaxPrice { get; init; }
    public decimal MinPrice { get; init; }
    public decimal LastPrice { get; init; }
}