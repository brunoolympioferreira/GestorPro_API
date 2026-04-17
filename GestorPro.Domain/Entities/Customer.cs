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

    public void Update(string name, string tradeName, CustomerStatusEnum status, ICollection<Address> addresses, ICollection<Contact> contacts)
    {
        Name = name;
        TradeName = tradeName;
        Status = status;
        SyncAddresses(addresses);
        SyncContacts(contacts);
    }

    private void SyncAddresses(ICollection<Address> incoming)
    {
        var toRemove = Addresses
            .Where(existing => !incoming.Any(i => i.Id == existing.Id))
            .ToList();

        foreach (var address in toRemove)
            Addresses.Remove(address);

        foreach (var address in incoming.Where(i => i.Id == Guid.Empty))
            Addresses.Add(address);
    }

    private void SyncContacts(ICollection<Contact> incoming)
    {
        var toRemove = Contacts
            .Where(existing => !incoming.Any(i => i.Id == existing.Id))
            .ToList();

        foreach (var contact in toRemove)
            Contacts.Remove(contact);

        foreach (var contact in incoming.Where(i => i.Id == Guid.Empty))
            Contacts.Add(contact);
    }
}
