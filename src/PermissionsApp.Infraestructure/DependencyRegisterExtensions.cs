using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PermissionsApp.Domain.Primitives;
using PermissionsApp.Domain.Repositories;
using PermissionsApp.Infraestructure.EventBuses;
using PermissionsApp.Infraestructure.ORM;
using PermissionsApp.Infraestructure.ORM.Interceptors;
using PermissionsApp.Infraestructure.Repositories;

namespace PermissionsApp.Infraestructure;

public static class DependencyRegisterExtensions
{
    public static IServiceCollection AddInfraestructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, SoftDeleteInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<MainDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IEventBus, KafkaEventBus>();

        return services;
    }
}
