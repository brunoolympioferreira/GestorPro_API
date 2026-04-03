using GestorPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorPro.Infra.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.TradeName)
            .IsRequired(false)
            .HasMaxLength(200);

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);



        builder.OwnsOne(c => c.Document, document =>
        {
            document.Property(d => d.Value)
                .HasColumnName("Document")
                .IsRequired()
                .HasMaxLength(18);

            document.Property(d => d.DocumentType)
                .HasColumnName("DocumentType")
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(4);

            document.Property(d => d.CustomerType)
                .HasColumnName("CustomerType")
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(2);

            document.HasIndex(d => d.Value)
                .IsUnique()
                .HasDatabaseName("IX_Customers_Document");
        });
    }
}
