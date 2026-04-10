using GestorPro.Application.Models.DTO;
using GestorPro.Domain.Entities;

namespace GestorPro.Application.Models.Mappers;

public static class AddressMapper
{
    public static AddressDTO ToDTO(this Address address) => new(
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
