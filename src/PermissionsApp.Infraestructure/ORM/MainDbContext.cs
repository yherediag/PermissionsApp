using Microsoft.EntityFrameworkCore;
using PermissionsApp.Domain.Entities;
using System.Reflection;

namespace PermissionsApp.Infraestructure.ORM;

public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
{
    public DbSet<Permission> Permissions => Set<Permission>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

//Add-Migration InitialDB -Context MainDbContext -Project src\PermissionsApp.Infraestructure -StartupProject src\PermissionsApp.API -OutputDir Migrations
//Update-Database -Context MainDbContext
//Remove-Migration -Context MainDbContext -Project src\PermissionsApp.Infraestructure