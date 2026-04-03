using GestorPro.Domain.Entities;

namespace GestorPro.Application.Models.DTO;

public record ContactDTO(Guid CustomerId, string Email, string Phone, bool IsPrimary)
{
    public Contact ToEntity() => new(CustomerId, Email, Phone, IsPrimary);
}
