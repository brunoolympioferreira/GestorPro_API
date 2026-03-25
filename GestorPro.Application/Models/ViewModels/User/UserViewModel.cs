namespace GestorPro.Application.Models.ViewModels.User;

public record UserViewModel(Guid Id, string Name, string Email, string Role, bool IsActive)
{
}
