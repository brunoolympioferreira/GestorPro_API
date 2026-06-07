using GestorPro.Application.Models.DTO;
using GestorPro.Domain.Enums;

namespace GestorPro.Application.Models.InputModels;

public record CreateCustomerInputModel(
    string Name,
    string TradeName,
    string Document,
    string Status,
    ICollection<AddressDTO> Addresses,
    ICollection<ContactDTO> Contacts
);

public record UpdateCustomerInputModel(
    Guid? Id,
    string Name,
    string TradeName,
    ICollection<AddressDTO> Addresses,
    ICollection<ContactDTO> Contacts
);
