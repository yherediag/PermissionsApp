namespace PermissionsApp.Domain.Repositories;

public interface IUnitOfWork
{
    public IPermissionsRepository Permissions { get; }
    public IPermissionsTypeRepository PermissionsType { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
