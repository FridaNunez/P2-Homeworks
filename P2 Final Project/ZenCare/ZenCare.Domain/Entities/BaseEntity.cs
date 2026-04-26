namespace ZenCare.Domain.Entities;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    protected BaseEntity(Guid id)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }

    public abstract string GetDescription();

    public string GetActiveStatus() => IsActive ? "Active" : "Inactive";
}