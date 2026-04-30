namespace GestorPro.Domain.Entities;

public sealed class Inventory : BaseEntity
{
    public Inventory(Guid productId, decimal currentQuantity, decimal minimumQuantity, decimal? maximumQuantity, string location, DateTime lastMovementAt)
    {
        ProductId = productId;
        CurrentQuantity = currentQuantity;
        MinimumQuantity = minimumQuantity;
        MaximumQuantity = maximumQuantity;
        Location = location;
        LastMovementAt = lastMovementAt;
    }

    protected Inventory() { } //EF Core

    public Guid ProductId { get; private set; }
    public decimal CurrentQuantity { get; private set; }
    public decimal MinimumQuantity { get; private set; }
    public decimal? MaximumQuantity { get; private set; }
    public string Location { get; private set; }
    public DateTime LastMovementAt { get; private set; }

    //Navigation properties
    public Product Product { get; private set; } = null!;
}
