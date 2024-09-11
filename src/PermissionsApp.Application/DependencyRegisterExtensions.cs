using Microsoft.Extensions.DependencyInjection;
using PermissionsApp.Application.Services.Implementations;
using PermissionsApp.Application.Services;
using System.Reflection;

namespace PermissionsApp.Application;

public static class DependencyRegisterExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped(typeof(IElasticsearchService<>), typeof(ElasticsearchService<>));

        return services;
    }
}
