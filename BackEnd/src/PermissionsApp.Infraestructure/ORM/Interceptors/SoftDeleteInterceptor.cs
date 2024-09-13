using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PermissionsApp.Domain.Primitives;

namespace PermissionsApp.Infraestructure.ORM.Interceptors;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            foreach (var entry in eventData.Context.ChangeTracker.Entries<ISoftDelete>())
            {
                if (entry is not { State: EntityState.Deleted, Entity: ISoftDelete delete }) 
                    continue;

                entry.State = EntityState.Modified;
                delete.IsDeleted = true;
                delete.DeletedDate = DateTime.UtcNow;
            }
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
