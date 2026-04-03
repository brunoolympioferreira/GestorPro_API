using GestorPro.Domain.Entities;
using GestorPro.Domain.Interfaces.Repositories;

namespace GestorPro.Infra.Persistence.Repositories;

public class CustomerRepository(AppDbContext context) : BaseRepository<Customer>(context), ICustomerRepository
{
}
