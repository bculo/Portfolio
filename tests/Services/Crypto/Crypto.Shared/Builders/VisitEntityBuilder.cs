using Crypto.Core.Entities;
using Tests.Common.Interfaces.Builders;

namespace Crypto.Shared.Builders;

public class VisitEntityBuilder : IObjectBuilder<VisitEntity>
{
    private readonly Guid _id = Guid.NewGuid();
    private Guid? _cryptoId = null!;

    public VisitEntity Build() => new()
    {
        Id = _id,
        CryptoId = _cryptoId ?? throw new ArgumentNullException(nameof(_cryptoId)),
    };

    public VisitEntityBuilder WithCryptoId(Guid cryptoId)
    {
        _cryptoId = cryptoId;
        return this;
    }
}