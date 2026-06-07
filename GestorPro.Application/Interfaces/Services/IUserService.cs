using GestorPro.Application.Models.InputModels;
using GestorPro.Application.Models.ViewModels;

namespace GestorPro.Application.Interfaces.Services;

public interface IUserService
{
    Task<Guid> CreateAsync(CreateUserInputModel inputModel, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, UpdateUserInputModel inputModel, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserViewModel?>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserViewModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
