namespace GestorPro.Application.Models.ViewModels.User;

public record LoginViewModel(string Token, DateTime ExpiresAt)
{
}
