using Confluent.Kafka;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PermissionsApp.WebAPI.HealthChecks;

public class KafkaHealthCheck(IConfiguration configuration) : IHealthCheck
{
    private readonly IConfiguration _configuration = configuration;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var config = new ProducerConfig { 
            BootstrapServers = _configuration["Kafka:BootstrapServers"],
            AllowAutoCreateTopics = true,
            Acks = Acks.All
        };

        try
        {
            using var producer = new ProducerBuilder<Null, string>(config).Build();

            var deliveryResult = await producer.ProduceAsync("health-check-topic",
                                                             new Message<Null, string> { Value = "health check" },
                                                             cancellationToken);

            // Check for errors in DeliveryResult
            if (deliveryResult.Status == PersistenceStatus.Persisted)
            {
                return HealthCheckResult.Healthy("Kafka connected!");
            }
            else
            {
                return HealthCheckResult.Unhealthy($"Kafka producer error: {deliveryResult?.Message?.Value ?? "Unknown error"}");
            }
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Exception: {ex.Message}");
        }
    }
}
