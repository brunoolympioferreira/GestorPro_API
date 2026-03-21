using GestorPro.Application.Models.InputModels.User;
using GestorPro.Application.Models.ViewModels.User;

namespace GestorPro.Application.Interfaces.Services;

public interface ILoginService
{
    Task<LoginViewModel> LoginAsync(LoginInputModel inputModel, CancellationToken cancellationToken = default);
}
