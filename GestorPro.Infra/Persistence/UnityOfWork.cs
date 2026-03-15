using GestorPro.Domain.Interfaces.Contracts;
using GestorPro.Domain.Interfaces.Repositories;

namespace GestorPro.Infra.Persistence;

public class UnityOfWork : IUnityOfWork
{
    private readonly AppDbContext _context;

    public UnityOfWork(
        AppDbContext context,
        IUserRepository userRepository)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        Users = userRepository;
    }

    public IUserRepository Users { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
