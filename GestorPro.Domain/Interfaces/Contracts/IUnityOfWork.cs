using GestorPro.Domain.Interfaces.Repositories;

namespace GestorPro.Domain.Interfaces.Contracts;

public interface IUnityOfWork
{
    IUserRepository Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
