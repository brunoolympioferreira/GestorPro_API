using GestorPro.Domain.Entities;

namespace GestorPro.Domain.Interfaces.Repositories;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<Role?> GetByNameAsync(string name);
}
