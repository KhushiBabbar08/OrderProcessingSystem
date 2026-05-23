namespace OrderProcessingSystem.DTOs;

public class CreateOrderDto
{
    public string CustomerName { get; set; }

    public string ProductName { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }
}
