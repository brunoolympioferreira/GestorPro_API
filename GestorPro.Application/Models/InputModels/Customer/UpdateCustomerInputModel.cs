using GestorPro.Application.Models.DTO;

namespace GestorPro.Application.Models.InputModels.Customer;

public record UpdateCustomerInputModel(
    Guid? Id,
    string Name, 
    string TradeName,
    ICollection<AddressDTO> Addresses,
    ICollection<ContactDTO> Contacts
);
