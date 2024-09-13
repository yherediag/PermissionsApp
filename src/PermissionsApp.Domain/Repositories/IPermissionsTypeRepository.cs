using PermissionsApp.Domain.Entities;

namespace PermissionsApp.Domain.Repositories;

public interface IPermissionsTypeRepository
{
    Task<IEnumerable<PermissionType>> GetAllAsync(CancellationToken cancellationToken);
}
