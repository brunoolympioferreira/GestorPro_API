using GestorPro.Domain.Entities;
using GestorPro.Domain.Enums;

namespace GestorPro.Application.Models.DTO;

public record AddressDTO(
    Guid CustomerId,
    string Street,
    string Number,
    string Complement,
    string Neighborhood,
    string City,
    string State,
    string ZipCode,
    string AddressType)
{
    public Address ToEntity()
    {
        var addressType = Enum.Parse<AddressTypeEnum>(AddressType, ignoreCase: true);
        return new Address(CustomerId, Street, Number, Complement, Neighborhood, City, State, ZipCode, addressType);
    }
}
