using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace PermissionsApp.Infraestructure.ORM;

public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

//Add-Migration InitialDB -Context MainDbContext -Project src\PermissionsApp.Infraestructure -StartupProject src\PermissionsApp.API -OutputDir Migrations
//Update-Database -Context MainDbContext
//Remove-Migration -Context MainDbContext -Project src\PermissionsApp.Infraestructure