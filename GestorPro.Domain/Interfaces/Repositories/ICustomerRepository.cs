using GestorPro.Domain.Entities;

namespace GestorPro.Domain.Interfaces.Repositories;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<Customer?> GetByIdAsync(Guid id, bool includeAddress, bool includeContact);
}
