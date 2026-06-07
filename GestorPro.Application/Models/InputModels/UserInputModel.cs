namespace GestorPro.Application.Models.InputModels;

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


public record UpdateUserInputModel(
    string Name,
    string Email,
    string Password,
    string Role
);

public record LoginInputModel(string Email, string Password);

