using ABO.ToDoApp.Domain.Entities;
using ABO.ToDoApp.Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ABO.ToDoApp.Infrastructure.Data.Interceptors;

public class SaveChangesInterceptor : ISaveChangesInterceptor
{

    public async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var contex = eventData.Context as ToDoAppContext;

        if (contex is null) return result;

        var tracker = contex!.ChangeTracker;

        var entries = tracker
                .Entries<IAuditable>()
                .Where(g => g.State == EntityState.Modified || g.State == EntityState.Added);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Property<DateTime>("UpdatedDate").CurrentValue = DateTime.UtcNow;
            }

            if (entry.State == EntityState.Added)
            {
                entry.Property<DateTime>("CreationDate").CurrentValue = DateTime.UtcNow;
                entry.Property<DateTime>("UpdatedDate").CurrentValue = DateTime.UtcNow;
            }
        }

        return await ValueTask.FromResult(result);
    }
}
