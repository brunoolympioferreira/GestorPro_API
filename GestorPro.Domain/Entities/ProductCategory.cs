namespace GestorPro.Domain.Entities;

public sealed class ProductCategory : BaseEntity
{
    public ProductCategory(string name, string? description, bool isActive)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
    }

    protected ProductCategory() { } //EF Core

    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    //Association properties
    public ICollection<Product> Products { get; private set; } = [];

    public void Update(string name, string? description, bool isActive)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        UpdateTimestamps();
    }
}
