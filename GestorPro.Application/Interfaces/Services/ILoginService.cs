using GestorPro.Application.Models.InputModels;
using GestorPro.Application.Models.ViewModels;

namespace GestorPro.Application.Interfaces.Services;

public interface ILoginService
{
    Task<LoginViewModel> LoginAsync(LoginInputModel inputModel, CancellationToken cancellationToken = default);
}
