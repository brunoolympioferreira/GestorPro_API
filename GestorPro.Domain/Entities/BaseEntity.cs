using GestorPro.Domain.Interfaces.Contracts;

namespace GestorPro.Domain.Entities;

public abstract class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public void UpdateTimestamps()
    {
        UpdatedAt = DateTime.Now;
    }
}
