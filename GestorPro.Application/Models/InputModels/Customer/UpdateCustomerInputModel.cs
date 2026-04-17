using GestorPro.Application.Models.DTO;

namespace GestorPro.Application.Models.InputModels.Customer;

public record UpdateCustomerInputModel(
    string Name, 
    string TradeName, 
    string Status, 
    ICollection<AddressDTO> Addresses,
    ICollection<ContactDTO> Contacts
);
