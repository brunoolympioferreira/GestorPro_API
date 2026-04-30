namespace GestorPro.Domain.Entities;

public sealed class Product : BaseEntity
{
    public Product(string sku, string name, string? description, Guid categoryId, Guid unityOfMeasureId, decimal costPrice, 
        decimal salePrice, bool isActive)
    {
        Sku = sku;
        Name = name;
        Description = description;
        CategoryId = categoryId;
        UnityOfMeasureId = unityOfMeasureId;
        CostPrice = costPrice;
        SalePrice = salePrice;
        IsActive = isActive;
    }

    protected Product() { } //EF Core

    public string Sku { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Guid CategoryId { get; private set; }
    public Guid UnityOfMeasureId { get; private set; }
    public decimal CostPrice { get; private set; }
    public decimal SalePrice { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    public ProductCategory Category { get; private set; } = null!;
    public UnitOfMeasure UnityOfMeasure { get; private set; } = null!;

    // Associations
    public Inventory Inventory { get; set; } = null!;
    public ICollection<InventoryMovement> InventoryMovements = [];
}
