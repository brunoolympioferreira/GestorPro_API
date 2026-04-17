using GestorPro.Application.Models.DTO;
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
}
