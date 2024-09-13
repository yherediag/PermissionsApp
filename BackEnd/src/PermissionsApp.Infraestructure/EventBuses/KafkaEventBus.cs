﻿using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PermissionsApp.Domain.Primitives;
using System.Text.Json;

namespace PermissionsApp.Infraestructure.EventBuses;

public class KafkaEventBus : IEventBus
{
    private readonly IProducer<Null, string> _producer;
    private readonly ILogger<KafkaEventBus> _logger;

    public KafkaEventBus(IConfiguration configuration,
                         ILogger<KafkaEventBus> logger)
    {
        _producer = new ProducerBuilder<Null, string>(new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"],
            AllowAutoCreateTopics = true,
            Acks = Acks.All
        }).Build();

        _logger = logger;
    }

    public void Dispose() => _producer?.Dispose();

    public async Task Publish<T>(T @event) where T : EventMessage
    {
        var topicName = "permissions";
        var message = new Message<Null, string>
        {
            Value = JsonSerializer.Serialize(@event)
        };

        try
        {
            var response = await _producer.ProduceAsync(topicName, message);

            _logger.LogInformation("Message '{Key}' sent to topic '{TopicPartitionOffset}'", message.Key, response.TopicPartitionOffset);
        }
        catch (ProduceException<string, string> ex)
        {
            _logger.LogError("An error occurred while publishing message to Kafka: {Reason}", ex.Error.Reason);
        }
    }
}
