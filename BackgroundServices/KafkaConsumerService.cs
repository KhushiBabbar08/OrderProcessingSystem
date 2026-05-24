using Confluent.Kafka;

namespace OrderProcessingSystem.BackgroundServices;

public class KafkaConsumerService : BackgroundService
{
    private readonly IConfiguration _configuration;

    public KafkaConsumerService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        Console.WriteLine(
            "Kafka Consumer Starting...");

        var config = new ConsumerConfig
        {
            BootstrapServers =
                _configuration["Kafka:BootstrapServers"],

            GroupId = "order-processing-group",

            AutoOffsetReset =
                AutoOffsetReset.Earliest,

            AllowAutoCreateTopics = true
        };

        using var consumer =
            new ConsumerBuilder<Ignore, string>(config)
                .Build();

        consumer.Subscribe("order-created-topic");

        Console.WriteLine(
            "Kafka Consumer Successfully Subscribed");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult =
                    consumer.Consume(
                        TimeSpan.FromSeconds(1));

                if (consumeResult != null)
                {
                    Console.WriteLine(
                        $"Kafka Event Received: {consumeResult.Message.Value}");
                }
            }
            catch (ConsumeException ex)
            {
                Console.WriteLine(
                    $"Kafka Consume Error: {ex.Error.Reason}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"General Error: {ex.Message}");
            }

            await Task.Delay(1000, stoppingToken);
        }

        consumer.Close();
    }
}