using GestorPro.Domain.Entities;
using GestorPro.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GestorPro.Infra.Persistence.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        User? user = await _dbSet
            .AsNoTracking()
            .Include(u => u.Role)
            .SingleOrDefaultAsync(u => u.Email.Value == email);

        return user;
    }
}
