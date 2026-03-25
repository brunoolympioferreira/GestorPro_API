using GestorPro.Domain.Entities;

namespace GestorPro.Domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsyncWithRole(Guid id);
    Task<IEnumerable<User?>> GetAllAsyncWithRole();
}
