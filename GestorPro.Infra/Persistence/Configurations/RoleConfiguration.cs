using GestorPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestorPro.Infra.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    // IDs fixos para garantir idempotência nas migrations
    public static readonly Guid AdminRoleId = new("11111111-0000-0000-0000-000000000001");
    public static readonly Guid ManagerRoleId = new("11111111-0000-0000-0000-000000000002");
    public static readonly Guid EmployeeRoleId = new("11111111-0000-0000-0000-000000000003");
    public static readonly Guid ViewerRoleId = new("11111111-0000-0000-0000-000000000004");
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(r => r.Description)
            .HasMaxLength(500);

        SeedData(builder);
    }

    private static void SeedData(EntityTypeBuilder<Role> builder)
    {
        var seedDate = new DateTime(2026, 3, 14, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new
            {
                Id = AdminRoleId,
                Name = "Admin",
                Description = "Acesso total ao sistema.",
                CreatedAt = seedDate,
            },
            new
            {
                Id = ManagerRoleId,
                Name = "Manager",
                Description = "Acesso gerencial.",
                CreatedAt = seedDate,
            },
            new
            {
                Id = EmployeeRoleId,
                Name = "Employee",
                Description = "Acesso padrão de colaborador.",
                CreatedAt = seedDate,
            },
            new
            {
                Id = ViewerRoleId,
                Name = "Viewer",
                Description = "Acesso somente leitura.",
                CreatedAt = seedDate,
            }
        );
    }
}
