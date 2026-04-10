using GestorPro.Application.Models.DTO;
using GestorPro.Domain.Entities;

namespace GestorPro.Application.Models.Mappers;

public static class ContactMapper
{
    public static ContactDTO ToContactDTO(this Contact contact) => new(
        contact.Email?.Value,
        contact.Phone,
        contact.IsPrimary
    );

    public static ICollection<ContactDTO?> ToContactDTOList(this IEnumerable<Contact> contacts) =>
        contacts.Select(c => (ContactDTO?)c.ToContactDTO()).ToList();
}
