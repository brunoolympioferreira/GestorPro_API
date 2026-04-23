using GestorPro.Application.Models.DTO;
using GestorPro.Domain.Entities;

namespace GestorPro.Application.Models.Mappers;

public static class ContactMapper
{
    public static Contact ToEntity(this ContactDTO dto, Guid customerId)
    {
        var contact = new Contact(customerId, dto.Email, dto.Phone, dto.IsPrimary);

        if (dto.Id.HasValue && dto.Id != Guid.Empty)
            contact.Id = dto.Id.Value;

        return contact;
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
