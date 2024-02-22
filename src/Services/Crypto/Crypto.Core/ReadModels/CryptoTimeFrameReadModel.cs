namespace Crypto.Core.ReadModels;

public class CryptoTimeFrameReadModel
{
    public Guid CryptoId { get; set; }
    public string Symbol { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Website { get; set; } = default!;
    public string SourceCode { get; set; } = default!;
    public DateTimeOffset TimeBucket { get; set; }
    public decimal AvgPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public decimal MinPrice { get; set; }
    public decimal LastPrice { get; set; }
}