using Confluent.Kafka;
using OrderProcessingSystem.Interfaces;
using System.Text.Json;

namespace OrderProcessingSystem.Services;

public class KafkaProducerService : IKafkaProducerService
{
    private readonly IConfiguration _configuration;

    public KafkaProducerService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task PublishOrderCreatedAsync(
        string topic,
        object message)
    {
        var config = new ProducerConfig
        {
            BootstrapServers =
                _configuration["Kafka:BootstrapServers"]
        };

        using var producer =
            new ProducerBuilder<Null, string>(config)
                .Build();

        var jsonMessage =
            JsonSerializer.Serialize(message);

        Console.WriteLine(
            "Publishing Kafka Message...");

        await producer.ProduceAsync(
            topic,
            new Message<Null, string>
            {
                Value = jsonMessage
            });

        Console.WriteLine(
            $"Kafka Message Published: {jsonMessage}");
    }
}