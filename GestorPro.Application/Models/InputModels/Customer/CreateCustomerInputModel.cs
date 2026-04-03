using GestorPro.Application.Models.DTO;
using GestorPro.Domain.Entities;
using GestorPro.Domain.Enums;

namespace GestorPro.Application.Models.InputModels.Customer;

public record CreateCustomerInputModel(
    string Name,
    string TradeName,
    string Document,
    string Status,
    ICollection<AddressDTO> Addresses,
    ICollection<ContactDTO> Contacts)
{
    public Domain.Entities.Customer ToEntity(ICollection<Address> addresses, ICollection<Contact> contacts)
    {
        var customerStatusEnum = Enum.Parse<CustomerStatusEnum>(Status, ignoreCase: true);
        var customer = new Domain.Entities.Customer(Name, TradeName, Document, customerStatusEnum)
        {
            Addresses = addresses,
            Contacts = contacts
        };

        return customer;
    }
}
