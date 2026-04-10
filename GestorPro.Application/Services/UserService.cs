using GestorPro.Application.Interfaces.Services;
using GestorPro.Application.Models.InputModels.User;
using GestorPro.Application.Models.ViewModels;
using GestorPro.Domain.Interfaces.Contracts;

namespace GestorPro.Application.Services;

public class UserService(IUnityOfWork unityOfWork, IAuthService authService) : IUserService
{
    public async Task<Guid> CreateAsync(CreateUserInputModel inputModel, CancellationToken cancellationToken = default)
    {
        var passwordHash = authService.ComputeSha256Hash(inputModel.Password);
        var role = await unityOfWork.Roles.GetByNameAsync(inputModel.Role) ?? throw new ArgumentNullException("role");

        var user = inputModel.ToEntity(passwordHash, role.Id);

        var existingEmail = await unityOfWork.Users.ExistsAsync(u => u.Email.Value == inputModel.Email);
        if (existingEmail)
        {
            throw new InvalidOperationException("Email já existe.");
        }

        await unityOfWork.Users.AddAsync(user);

        await unityOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }

    public async Task UpdateAsync(Guid id, UpdateUserInputModel inputModel, CancellationToken cancellationToken = default)
    {
        var user = await unityOfWork.Users.GetByIdAsyncWithRoleNoTracking(id)
            ?? throw new KeyNotFoundException();

        var role = await unityOfWork.Roles.GetByNameAsync(inputModel.Role) ?? throw new ArgumentNullException("role");

        var passwordHash = authService.ComputeSha256Hash(inputModel.Password);

        user.Update(inputModel.Name, inputModel.Email, passwordHash, role.Id);

        await unityOfWork.Users.UpdateAsync(user);
        await unityOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserViewModel?>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await unityOfWork.Users.GetAllAsyncWithRole();

        if (!users.Any()) return [];

        var viewModels = users.Select(u => new UserViewModel(u.Id, u.Name, u.Email.Value, u.Role.Name, u.IsActive));

        return viewModels;
    }

    public async Task<UserViewModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await unityOfWork.Users.GetByIdAsyncWithRole(id)
            ?? throw new KeyNotFoundException();

        var viewModel = new UserViewModel(user.Id, user.Name, user.Email.Value, user.Role.Name, user.IsActive);

        return viewModel;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await unityOfWork.Users.GetByIdAsyncWithRoleNoTracking(id)
            ?? throw new KeyNotFoundException();

        user.Delete();

        await unityOfWork.Users.UpdateAsync(user);
        await unityOfWork.SaveChangesAsync(cancellationToken);
    }
}
