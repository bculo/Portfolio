namespace Crypto.Core.Entities;

public class CryptoPrice
{
    public DateTimeOffset Time { get; init; }
    
    public decimal Price { get; init; }
    
    public Guid CryptoId { get; init; }
    public CryptoEntity Crypto { get; init; } = default!;
}

