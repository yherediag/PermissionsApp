using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PermissionsApp.Domain.Repositories;
using PermissionsApp.Infraestructure.ORM;
using PermissionsApp.Infraestructure.Repositories;

namespace PermissionsApp.Infraestructure;

public static class DependencyRegisterExtensions
{
    public static IServiceCollection AddInfraestructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MainDbContext>((sp, options) =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
