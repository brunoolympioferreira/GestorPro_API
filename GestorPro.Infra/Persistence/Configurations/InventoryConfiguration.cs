using GestorPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorPro.Infra.Persistence.Configurations;

public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.ToTable("Inventories");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.CurrentQuantity)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(i => i.MinimumQuantity)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(i => i.MaximumQuantity)
            .HasPrecision(18, 2);

        builder.Property(i => i.Location)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.LastMovementAt)
            .IsRequired();

        builder.HasOne(i => i.Product)
            .WithOne(p => p.Inventory)
            .HasForeignKey<Inventory>(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
