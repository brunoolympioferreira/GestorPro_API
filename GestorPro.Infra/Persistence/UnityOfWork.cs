using GestorPro.Domain.Interfaces.Contracts;
using GestorPro.Domain.Interfaces.Repositories;

namespace GestorPro.Infra.Persistence;

public class UnityOfWork : IUnityOfWork
{
    private readonly AppDbContext _context;

    public UnityOfWork(
        AppDbContext context,
        IUserRepository userRepository,
        IRoleRepository roles,
        ICustomerRepository customers)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        Users = userRepository;
        Roles = roles;
        Customers = customers;
    }

    public IUserRepository Users { get; }
    public IRoleRepository Roles { get; }
    public ICustomerRepository Customers { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
