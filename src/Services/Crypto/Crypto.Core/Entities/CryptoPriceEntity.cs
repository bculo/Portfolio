namespace Crypto.Core.Entities;

public class CryptoPriceEntity
{
    public DateTimeOffset Time { get; init; }
    public decimal Price { get; init; }
    public Guid CryptoEntityId { get; init; }
    public CryptoEntity CryptoEntity { get; init; } = default!;
}

