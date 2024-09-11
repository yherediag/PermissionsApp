using PermissionsApp.Domain.Entities;

namespace PermissionsApp.Domain.Repositories;

public interface IPermissionsRepository
{
    Task<IEnumerable<Permission>> GetAllAsync(CancellationToken cancellationToken);
    Task<Permission?> GetByIdAsync(int permissionId, CancellationToken cancellationToken);
    Task AddAsync(Permission permission, CancellationToken cancellationToken);
    void Update(Permission permission);
    void Delete(Permission permission);
}
