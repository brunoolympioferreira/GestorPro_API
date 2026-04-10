using GestorPro.Domain.Entities;
using GestorPro.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GestorPro.Infra.Persistence.Repositories;

public class CustomerRepository(AppDbContext context) : BaseRepository<Customer>(context), ICustomerRepository
{
    public async Task<Customer?> GetByIdAsync(Guid id, bool includeAddress, bool includeContact)
    {
        IQueryable<Customer> query = _dbSet.AsNoTracking();

        if (includeAddress)
            query = query.Include(c => c.Addresses);

        if (includeContact)
            query = query.Include(c => c.Contacts);

        return await query.FirstOrDefaultAsync(c => c.Id == id);
    }
}
