using GestorPro.Application.Models.InputModels.User;

namespace GestorPro.Application.Interfaces.Services;

public interface IUserService
{
    Task<Guid> CreateAsync(CreateUserInputModel inputModel, CancellationToken cancellationToken = default);
}
