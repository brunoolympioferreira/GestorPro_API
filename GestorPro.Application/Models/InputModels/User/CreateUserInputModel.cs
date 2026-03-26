namespace GestorPro.Application.Models.InputModels.User;

public record CreateUserInputModel(
    string Name,
    string Email,
    string Password,
    string Role,
    bool IsActive)
{
    public Domain.Entities.User ToEntity(string passwordHash, Guid roleId)
        => new(Name, Email, passwordHash, roleId, IsActive);
}
