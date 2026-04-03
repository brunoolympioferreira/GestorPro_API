using GestorPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorPro.Infra.Persistence.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .IsRequired();

        builder.Property(c => c.CustomerId)
            .IsRequired();

        builder.OwnsOne(c => c.Email, email =>
        {
            email.Property(e => e.Value)
                .IsRequired(false)
                .HasColumnName("Email")
                .HasMaxLength(254);
        });

        builder.Property(c => c.Phone)
            .IsRequired(false)
            .HasMaxLength(20);

        builder.Property(c => c.IsPrimary)
            .IsRequired();

        builder.HasOne<Customer>()
            .WithMany(c => c.Contacts)
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
