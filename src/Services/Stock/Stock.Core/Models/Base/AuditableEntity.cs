namespace Stock.Core.Models.Base
{
    public abstract class AuditableEntity : Entity
    {
        public string CreatedBy { get; set; } = default!;
        public string ModifiedBy { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Deactivated { get; set; }
    }
}
