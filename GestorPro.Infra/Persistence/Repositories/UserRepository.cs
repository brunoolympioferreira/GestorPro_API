using GestorPro.Domain.Entities;
using GestorPro.Domain.Interfaces.Repositories;

namespace GestorPro.Infra.Persistence.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
{
}
