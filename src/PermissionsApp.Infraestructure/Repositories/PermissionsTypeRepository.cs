using Microsoft.EntityFrameworkCore;
using PermissionsApp.Domain.Entities;
using PermissionsApp.Domain.Repositories;
using PermissionsApp.Infraestructure.ORM;

namespace PermissionsApp.Infraestructure.Repositories;

public class PermissionsTypeRepository(MainDbContext dbContext) : IPermissionsTypeRepository
{
    private readonly DbSet<PermissionType> PermissionType = dbContext.Set<PermissionType>();

    public async Task<IEnumerable<PermissionType>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await PermissionType
            .ToListAsync(cancellationToken);
    }
}
