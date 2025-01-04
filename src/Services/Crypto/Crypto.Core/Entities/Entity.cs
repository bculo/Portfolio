namespace Crypto.Core.Entities;

public abstract class Entity
{
    public Guid Id { get; set; }
    public DateTimeOffset ModifiedOn { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
}

