namespace GestorPro.Application.Models.DTO;

public record ContactDTO(Guid CustomerId, string Email, string Phone, bool IsPrimary)
{
}
