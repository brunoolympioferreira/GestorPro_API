using GestorPro.Domain.Entities;
using GestorPro.Domain.Enums;

namespace GestorPro.Application.Models.DTO;

public record AddressDTO(
    string Street,
    string Number,
    string Complement,
    string Neighborhood,
    string City,
    string State,
    string ZipCode,
    string AddressType)
{
    public Address ToEntity(Guid customerId)
    {
        var addressType = Enum.Parse<AddressTypeEnum>(AddressType, ignoreCase: true);
        return new Address(customerId, Street, Number, Complement, Neighborhood, City, State, ZipCode, addressType);
    }
}
