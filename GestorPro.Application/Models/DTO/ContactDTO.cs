using GestorPro.Domain.Entities;
using GestorPro.Domain.ValueObjects;

namespace GestorPro.Application.Models.DTO;

public record ContactDTO(string Email, string Phone, bool IsPrimary)
{
    public Contact ToEntity(Guid customerId)
    {
        return new(customerId, Email, Phone, IsPrimary);
    }
}
