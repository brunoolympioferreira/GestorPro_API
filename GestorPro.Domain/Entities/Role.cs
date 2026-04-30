namespace GestorPro.Domain.Entities;

public sealed class Role : BaseEntity
{
    public Role(string name, string description)
    {
        Name = name;
        Description = description;
    }

    protected Role() { } // ← EF Core

    public string Name { get; private set; }
    public string Description { get; private set; }

    /// <summary>
    /// Associação com User - Um Role pode ter muitos Users
    /// </summary>
    public ICollection<User> Users { get; private set; } = [];
}
