using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PermissionsApp.Application;

public static class DependencyRegisterExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}
