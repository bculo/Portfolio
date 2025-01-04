namespace Crypto.Core.Entities;

public class VisitEntity : Entity
{
    public Guid CryptoId { get; set; }
    public CryptoEntity Crypto { get; set; } = default!;
}

