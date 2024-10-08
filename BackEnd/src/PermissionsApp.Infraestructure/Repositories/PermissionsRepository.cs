﻿using Microsoft.EntityFrameworkCore;
using PermissionsApp.Domain.Entities;
using PermissionsApp.Domain.Repositories;
using PermissionsApp.Infraestructure.ORM;

namespace PermissionsApp.Infraestructure.Repositories;

public class PermissionsRepository(MainDbContext dbContext) : IPermissionsRepository
{
    private readonly DbSet<Permission> Permissions = dbContext.Set<Permission>();

    public async Task<(IEnumerable<Permission> Permissions, int TotalCount)> GetAllAsync(int pageNumber,
                                                                                         int pageSize,
                                                                                         CancellationToken cancellationToken)
    {
        var query = Permissions.AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);
        var permissions = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(permission => permission.PermissionType)
            .ToListAsync(cancellationToken);

        return (permissions, totalCount);
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
