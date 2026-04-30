using GestorPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorPro.Infra.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Sku)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.CostPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.SalePrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(p => p.IsActive)
            .IsRequired();

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.UnityOfMeasure)
            .WithMany(um => um.Products)
            .HasForeignKey(p => p.UnityOfMeasureId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
