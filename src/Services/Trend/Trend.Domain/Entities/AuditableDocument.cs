namespace Trend.Domain.Entities;

public class AuditableDocument : RootDocument
{
    public DateTime? DeactivationDate { get; set; }
    public bool IsActive { get; set; }
}