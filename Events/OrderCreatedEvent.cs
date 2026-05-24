namespace OrderProcessingSystem.Events;

public class OrderCreatedEvent
{
    public Guid OrderId { get; set; }

    public string CustomerName { get; set; }

    public string ProductName { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string Status { get; set; }

    public DateTime CreatedDate { get; set; }
}