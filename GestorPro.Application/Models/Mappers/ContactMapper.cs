using GestorPro.Application.Models.DTO;
using GestorPro.Domain.Entities;

namespace GestorPro.Application.Models.Mappers;

public static class ContactMapper
{
    public static Contact ToEntity(this ContactDTO dto, Guid customerId)
    {
        return new(customerId, dto.Email, dto.Phone, dto.IsPrimary);
    }

    public static ContactDTO ToContactDTO(this Contact contact) => new(
        contact.Id,
        contact.Email?.Value,
        contact.Phone,
        contact.IsPrimary
    );

    public static ICollection<ContactDTO?> ToContactDTOList(this IEnumerable<Contact> contacts) =>
        contacts.Select(c => (ContactDTO?)c.ToContactDTO()).ToList();
}
