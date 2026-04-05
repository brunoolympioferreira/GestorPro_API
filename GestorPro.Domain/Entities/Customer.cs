using GestorPro.Domain.Enums;
using GestorPro.Domain.ValueObjects;

namespace GestorPro.Domain.Entities;

public class Customer : BaseEntity
{
    public Customer(string name, string tradeName, string document, CustomerStatusEnum status)
    {
        Name = name;
        TradeName = tradeName;
        Document = Document.Create(document);
        Status = status;
    }

    protected Customer() { } //EF Core

    public string Name { get; private set; }
    public string TradeName { get; private set; }
    public Document Document { get; private set; }
    public CustomerStatusEnum Status { get; private set; }

    public ICollection<Address> Addresses { get; private set; } = [];
    public ICollection<Contact> Contacts { get; private set; } = [];

    public void AddContacts(ICollection<Contact> contacts)
    {
        Contacts = contacts;
    }

    public void AddAddresses(ICollection<Address> addresses)
    {
        Addresses = addresses;
    }
}
