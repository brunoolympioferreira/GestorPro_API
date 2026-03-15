using GestorPro.Application.Interfaces.Services;
using GestorPro.Application.Models.InputModels.User;
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

        return user.Id;
    }
}
