namespace GestorPro.Application.Models.ViewModels;

public record UserViewModel(Guid Id, string Name, string Email, string Role, bool IsActive);
