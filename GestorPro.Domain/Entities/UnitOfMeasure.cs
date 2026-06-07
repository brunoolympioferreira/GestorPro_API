namespace GestorPro.Domain.Entities;

public sealed class UnitOfMeasure : BaseEntity
{
    public UnitOfMeasure(string code, string name, bool isActive)
    {
        Code = code;
        Name = name;
        IsActive = isActive;
    }

    protected UnitOfMeasure() { } //EF Core

    public string Code { get; private set; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }

    //Association properties
    public ICollection<Product> Products { get; private set; } = [];

    public void Update(string code, string name, bool isActive)
    {
        Code = code;
        Name = name;
        IsActive = isActive;
        UpdateTimestamps();
    }
}
