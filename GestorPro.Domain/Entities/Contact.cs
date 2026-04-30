using GestorPro.Domain.ValueObjects;

namespace GestorPro.Domain.Entities;

public sealed class Contact : BaseEntity
{
    public Contact(Guid customerId, string? email, string? phone, bool isPrimary)
    {
        CustomerId = customerId;
        Email = string.IsNullOrWhiteSpace(email) ? null : Email.Create(email);
        Phone = phone;
        IsPrimary = isPrimary;
    }

    protected Contact() { } //EF Core
    public Guid CustomerId { get; private set; }
    public Email? Email { get; private set; }
    public string? Phone { get; private set; }
    public bool IsPrimary { get; private set; }

    public void Update(string? email, string? phone, bool isPrimary)
    {
        Email = string.IsNullOrWhiteSpace(email) ? null : Email.Create(email);
        Phone = phone;
        IsPrimary = isPrimary;
    }
}
