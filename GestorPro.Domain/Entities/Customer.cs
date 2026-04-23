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
        foreach (var contact in contacts)
            Contacts.Add(contact);
    }

    public void AddAddresses(ICollection<Address> addresses)
    {
        foreach (var address in addresses)
            Addresses.Add(address);
    }

    public void Update(string name, string tradeName, ICollection<Address> addresses, ICollection<Contact> contacts)
    {
        Name = name;
        TradeName = tradeName;
        SyncAddresses(addresses);
        SyncContacts(contacts);
    }

    private void SyncAddresses(ICollection<Address> incoming)
    {
        var existingIds = Addresses.Select(a => a.Id).ToHashSet();
        var incomingIds = incoming.Select(a => a.Id).ToHashSet();

        var toRemove = Addresses.Where(a => !incomingIds.Contains(a.Id)).ToList();
        foreach (var address in toRemove)
            Addresses.Remove(address);

        foreach (var address in incoming)
        {
            var existing = Addresses.FirstOrDefault(a => a.Id == address.Id);
            if (existing is null)
                Addresses.Add(address);
            else
                existing.Update(address.Street, address.Number, address.Complement,
                    address.Neighborhood, address.City, address.State, address.ZipCode, address.AddressType);
        }
    }

    private void SyncContacts(ICollection<Contact> incoming)
    {
        var incomingIds = incoming.Select(c => c.Id).ToHashSet();

        var toRemove = Contacts.Where(c => !incomingIds.Contains(c.Id)).ToList();
        foreach (var contact in toRemove)
            Contacts.Remove(contact);

        foreach (var contact in incoming)
        {
            var existing = Contacts.FirstOrDefault(c => c.Id == contact.Id);
            if (existing is null)
                Contacts.Add(contact);
            else
                existing.Update(contact.Email?.Value, contact.Phone, contact.IsPrimary);
        }
    }
}
