using GestorPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorPro.Infra.Persistence.Configurations;

public class InventoryMovementConfiguration : IEntityTypeConfiguration<InventoryMovement>
{
    public void Configure(EntityTypeBuilder<InventoryMovement> builder)
    {
        builder.ToTable("InventoryMovements");

        builder.HasKey(im => im.Id);

        builder.Property(im => im.MovementType)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(im => im.Quantity)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(im => im.UnitCost)
            .HasPrecision(18, 2);

        builder.Property(im => im.Reason)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(im => im.ReferenceDocument)
            .HasMaxLength(50);

        builder.Property(im => im.Notes)
            .HasMaxLength(500);

        builder.HasOne(im => im.Product)
            .WithMany(p => p.InventoryMovements)
            .HasForeignKey(im => im.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
