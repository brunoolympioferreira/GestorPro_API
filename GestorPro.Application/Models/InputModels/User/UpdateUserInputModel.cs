namespace GestorPro.Application.Models.InputModels.User;

public record UpdateUserInputModel(
    string Name,
    string Email,
    string Password,
    string Role)
{
}
