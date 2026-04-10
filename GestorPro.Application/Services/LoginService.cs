using GestorPro.Application.Interfaces.Services;
using GestorPro.Application.Models.DTO;
using GestorPro.Application.Models.InputModels.User;
using GestorPro.Application.Models.ViewModels;
using GestorPro.Domain.Interfaces.Contracts;

namespace GestorPro.Application.Services;

public class LoginService(IUnityOfWork unityOfWork, IAuthService authService) : ILoginService
{
    public async Task<LoginViewModel> LoginAsync(LoginInputModel inputModel, CancellationToken cancellationToken = default)
    {
        var user = await unityOfWork.Users.GetByEmailAsync(inputModel.Email);

        if (user is null || !authService.VerifyPassword(inputModel.Password, user.PasswordHash))
            throw new UnauthorizedAccessException();

       TokenDTO token = authService.GenerateJwtToken(user);

       return new LoginViewModel(token.token, token.ExpiresAt);
    }
}
