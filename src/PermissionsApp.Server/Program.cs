using PermissionsApp.API.ExceptionHandlers;
using PermissionsApp.API.HealthChecks;
using PermissionsApp.Application;
using PermissionsApp.Infraestructure;

namespace PermissionsApp.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var configuration = builder.Configuration;

        builder.Services.AddApplicationLayer();
        builder.Services.AddInfraestructureLayer(configuration);

        builder.Services.AddHealthChecks();
        builder.Services.AddHealthChecks().AddCheck<SqlHealthCheck>(nameof(SqlHealthCheck));

        builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
        builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseExceptionHandler();

        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}
