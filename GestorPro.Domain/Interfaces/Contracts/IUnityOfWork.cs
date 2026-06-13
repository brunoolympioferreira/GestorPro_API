using GestorPro.Domain.Interfaces.Repositories;

namespace GestorPro.Domain.Interfaces.Contracts;

public interface IUnityOfWork
{
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    ICustomerRepository Customers { get; }
    IUnitOfMeasureRepository UnitOfMeasures { get; }
    IProductCategoryRepository ProductCategories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
