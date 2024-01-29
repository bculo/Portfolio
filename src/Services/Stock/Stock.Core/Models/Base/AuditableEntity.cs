namespace Stock.Core.Models.Base
{
    public abstract class AuditableEntity : IEntity
    {
        public string CreatedBy { get; set; } = default!;
        public string ModifiedBy { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
