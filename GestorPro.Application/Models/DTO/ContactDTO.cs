namespace GestorPro.Application.Models.DTO;

public record ContactDTO(Guid? Id, string? Email, string? Phone, bool IsPrimary);
