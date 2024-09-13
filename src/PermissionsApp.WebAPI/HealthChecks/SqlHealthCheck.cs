using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PermissionsApp.WebAPI.HealthChecks;

public class SqlHealthCheck(IConfiguration configuration) : IHealthCheck
{
    private readonly IConfiguration _configuration = configuration;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = new(_configuration.GetConnectionString("DefaultConnection"));
		try
		{
			await connection.OpenAsync(cancellationToken);
			await connection.CloseAsync();

			return HealthCheckResult.Healthy("Database connected!");
		}
		catch (Exception ex)
		{
			return HealthCheckResult.Unhealthy(ex.Message);
		}
    }
}
