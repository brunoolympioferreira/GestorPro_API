using GestorPro.Domain.ValueObjects;

namespace GestorPro.Domain.Entities;

public class User : BaseEntity
{
    public User(string name, string email, string passwordHash, Guid roleId, bool isActive)
    {
        Name = name;
        Email = Email.Create(email);
        PasswordHash = passwordHash;
        RoleId = roleId;
        IsActive = isActive;
    }

    protected User() { } // EF Core

    public string Name { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public Guid RoleId { get; private set; }
    public bool IsActive { get; private set; }

    /// <summary>
    /// Associação com Role - Um User tem um Role
    /// </summary>
    public Role Role { get; set; }
}
