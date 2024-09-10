using PermissionsApp.Domain.Repositories;
using PermissionsApp.Infraestructure.ORM;

namespace PermissionsApp.Infraestructure.Repositories;

public class PermissionsRepository(MainDbContext dbContext) : IPermissionsRepository
{
    private readonly MainDbContext _dbContext = dbContext;
}
