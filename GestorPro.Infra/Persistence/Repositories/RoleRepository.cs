using GestorPro.Domain.Entities;
using GestorPro.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GestorPro.Infra.Persistence.Repositories;

public class RoleRepository(AppDbContext context) : BaseRepository<Role>(context), IRoleRepository
{
    public async Task<Role?> GetByNameAsync(string name)
    {
        var role = await _dbSet.FirstOrDefaultAsync(r => r.Name == name);

        return role;
    }
}
