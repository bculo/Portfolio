namespace Stock.Core.Models.Base
{
    public abstract class AuditableEntity : Entity
    {
        public string CreatedBy { get; set; } = default!;
        public string ModifiedBy { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public DateTimeOffset? Deactivated { get; set; }
        public bool IsActive { get; set; }
    }
}
