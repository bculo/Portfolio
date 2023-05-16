namespace Mail.Application.Entities;

public abstract class BaseEntity<T>
{
    public T Id { get; set; }
    public DateTime Created { get; set; }
    public bool IsActive { get; set; }
}