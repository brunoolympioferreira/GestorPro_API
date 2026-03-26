using GestorPro.Domain.Entities;
using GestorPro.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GestorPro.Infra.Persistence.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<IEnumerable<User?>> GetAllAsyncWithRole()
        => await WithRole().ToListAsync();

    public async Task<User?> GetByEmailAsync(string email)
        => await WithRole().SingleOrDefaultAsync(u => u.Email.Value == email);

    public async Task<User?> GetByIdAsyncWithRole(Guid id)
        => await WithRole().FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User?> GetByIdAsyncWithRoleNoTracking(Guid id)
    => await WithRoleNoTracking().FirstOrDefaultAsync(u => u.Id == id);

    private IQueryable<User> WithRole()
        => _dbSet.AsNoTracking().Include(u => u.Role);

    private IQueryable<User> WithRoleNoTracking()
    => _dbSet.Include(u => u.Role);
}
