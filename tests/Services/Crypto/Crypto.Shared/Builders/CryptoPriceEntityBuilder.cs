using Bogus;
using Crypto.Core.Entities;
using Tests.Common.Interfaces.Builders;

namespace Crypto.Shared.Builders;

public class CryptoPriceEntityBuilder : IObjectBuilder<CryptoPriceEntity>
{
    public DateTimeOffset PriceOn { get; private set; } = DateTimeOffset.UtcNow;
    public decimal Price { get; private set; } = new Faker().Random.Decimal(1, 50000);
    public Guid? CryptoEntityId { get; private set; }
    
    
    public CryptoPriceEntity Build() => new CryptoPriceEntity
    {
        CryptoEntityId = CryptoEntityId ?? throw new ArgumentNullException(nameof(CryptoEntityId)),
        Price = Price,
        Time = PriceOn
    };

    public CryptoPriceEntityBuilder WithCryptoItemId(Guid cryptoEntityId)
    {
        CryptoEntityId = cryptoEntityId;
        return this;
    }
}