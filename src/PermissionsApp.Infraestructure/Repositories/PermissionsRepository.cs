using Microsoft.EntityFrameworkCore;
using PermissionsApp.Domain.Entities;
using PermissionsApp.Domain.Repositories;
using PermissionsApp.Infraestructure.ORM;

namespace PermissionsApp.Infraestructure.Repositories;

public class PermissionsRepository(MainDbContext dbContext) : IPermissionsRepository
{
    private readonly DbSet<Permission> Permissions = dbContext.Set<Permission>();

    public async Task<IEnumerable<Permission>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Permissions
            .Include(permission => permission.PermissionType)
            .ToListAsync(cancellationToken);
    }

    public async Task<Permission?> GetByIdAsync(int permissionId, CancellationToken cancellationToken)
    {
        return await Permissions
            .Include(permission => permission.PermissionType)
            .SingleOrDefaultAsync(permission => permission.PermissionId == permissionId, cancellationToken);
    }

    public async Task AddAsync(Permission permission, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(permission);

        await Permissions
            .AddAsync(permission, cancellationToken);
    }

    public void Update(Permission permission)
    {
        ArgumentNullException.ThrowIfNull(permission);

        Permissions.Update(permission);
    }

    public void Delete(Permission permission)
    {
        ArgumentNullException.ThrowIfNull(permission);

        Permissions.Remove(permission);
    }
}
