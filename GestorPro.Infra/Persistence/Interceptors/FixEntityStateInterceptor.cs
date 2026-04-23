using GestorPro.Domain.Interfaces.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace GestorPro.Infra.Persistence.Interceptors;

public class FixEntityStateInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
    DbContextEventData eventData,
    InterceptionResult<int> result,
    CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            await FixStatesAsync(eventData.Context, cancellationToken);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static async Task FixStatesAsync(DbContext context, CancellationToken cancellationToken)
    {
        var modifiedEntries = context.ChangeTracker
            .Entries<IBaseEntity>()
            .Where(e => e.State == EntityState.Modified)
            .ToList();

        foreach (var entry in modifiedEntries)
        {
            var dbValues = await entry.GetDatabaseValuesAsync(cancellationToken);
            if (dbValues is null)
                entry.State = EntityState.Added;
        }
    }
}
