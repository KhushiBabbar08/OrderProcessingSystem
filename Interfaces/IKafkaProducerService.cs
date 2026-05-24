namespace OrderProcessingSystem.Interfaces;

public interface IKafkaProducerService
{
    Task PublishOrderCreatedAsync(
        string topic,
        object message);
}