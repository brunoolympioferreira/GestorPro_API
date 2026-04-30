using GestorPro.Domain.Enums;

namespace GestorPro.Domain.Entities;

public sealed class InventoryMovement : BaseEntity
{
    public InventoryMovement(Guid productId, MovementTypeEnum movementType, decimal quantity, decimal? unitCost, string reason, string? referenceDocument, string? notes)
    {
        ProductId = productId;
        MovementType = movementType;
        Quantity = quantity;
        UnitCost = unitCost;
        Reason = reason;
        ReferenceDocument = referenceDocument;
        Notes = notes;
    }

    protected InventoryMovement() { } //EF Core

    public Guid ProductId { get; private set; }
    public MovementTypeEnum MovementType { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal? UnitCost { get; private set; }
    public string Reason { get; private set; }
    public string? ReferenceDocument { get; private set; }
    public string? Notes { get; private set; }

    // Navigation property
    public Product Product { get; private set; } = null!;
}
