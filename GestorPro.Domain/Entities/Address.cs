using GestorPro.Domain.Enums;

namespace GestorPro.Domain.Entities;

public class Address : BaseEntity
{
    public Address(Guid customerId, string street, string number, string complement, 
        string neighborhood, string city, string state, string cEP, AddressTypeEnum addressType)
    {
        CustomerId = customerId;
        Street = street;
        Number = number;
        Complement = complement;
        Neighborhood = neighborhood;
        City = city;
        State = state;
        CEP = cEP;
        AddressType = addressType;
    }

    protected Address() { } //EF Core

    public Guid CustomerId { get; private set; }
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string Complement { get; private set; }
    public string Neighborhood { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string CEP { get; private set; }
    public AddressTypeEnum AddressType { get; private set; }
}
