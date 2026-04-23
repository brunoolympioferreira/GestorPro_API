using GestorPro.Application.Models.DTO;
using GestorPro.Domain.Entities;
using GestorPro.Domain.Enums;

namespace GestorPro.Application.Models.Mappers;

public static class AddressMapper
{
    public static Address ToEntity(this AddressDTO dto, Guid customerId)
    {
        var addressType = Enum.Parse<AddressTypeEnum>(dto.AddressType, ignoreCase: true);
        var address = new Address(customerId, dto.Street, dto.Number, dto.Complement,
            dto.Neighborhood, dto.City, dto.State, dto.ZipCode, addressType);

        if (dto.Id.HasValue && dto.Id != Guid.Empty)
            address.Id = dto.Id.Value;

        return address;
    }

    public static AddressDTO ToDTO(this Address address) => new(
        address.Id,
        address.Street,
        address.Number,
        address.Complement,
        address.Neighborhood,
        address.City,
        address.State,
        address.ZipCode,
        address.AddressType.ToString()
    );

    public static ICollection<AddressDTO?> ToAddressDTOList(this IEnumerable<Address> addresses) =>
        addresses.Select(a => (AddressDTO?)a.ToDTO()).ToList();
}
