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

        public void ChangeStatus(DateTimeOffset time)
        {
            if(IsActive)
                Deactivate(time);
            else
                Activate();
        }
        
        public void Deactivate(DateTimeOffset deactivatedOn)
        {
            IsActive = false;
            Deactivated = deactivatedOn;
        }

        public void Activate()
        {
            IsActive = true;
            Deactivated = null;
        }
    }
}
