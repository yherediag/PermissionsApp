namespace PermissionsApp.Domain.Repositories;

public interface IUnitOfWork
{
    public IPermissionsRepository Permissions { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken);
}
