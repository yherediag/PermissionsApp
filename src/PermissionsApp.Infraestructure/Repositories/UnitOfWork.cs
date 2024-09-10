using PermissionsApp.Domain.Repositories;
using PermissionsApp.Infraestructure.ORM;

namespace PermissionsApp.Infraestructure.Repositories;

public class UnitOfWork(MainDbContext dbContext) : IUnitOfWork
{
    private readonly MainDbContext _dbContext = dbContext;

    private IPermissionsRepository? _permissions = null;

    public IPermissionsRepository Permissions => _permissions is null ?
            _permissions = new PermissionsRepository(_dbContext) :
            _permissions;

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
