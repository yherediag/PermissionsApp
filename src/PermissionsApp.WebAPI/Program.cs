using Microsoft.EntityFrameworkCore;
using PermissionsApp.Application;
using PermissionsApp.Infraestructure;
using PermissionsApp.Infraestructure.ORM;
using PermissionsApp.WebAPI.ExceptionHandlers;
using PermissionsApp.WebAPI.HealthChecks;

namespace PermissionsApp.WebAPI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var configuration = builder.Configuration;

        builder.Services.AddApplicationLayer();
        builder.Services.AddInfraestructureLayer(configuration);

        builder.Services.AddHealthChecks()
            .AddCheck<SqlHealthCheck>(nameof(SqlHealthCheck))
            .AddCheck<KafkaHealthCheck>(nameof(KafkaHealthCheck), tags: ["kafka"]);

        builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
        builder.Services.AddExceptionHandler<BadRequestExceptionHandler>();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();

        builder.Services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();
            });
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<MainDbContext>();
            await dbContext.Database.MigrateAsync();
        }

        app.UseDefaultFiles();
        app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors();

        app.UseAuthorization();
        app.UseExceptionHandler();

        app.MapControllers();

        app.MapHealthChecks("/health");

        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}
