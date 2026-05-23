using System.ComponentModel.DataAnnotations;

namespace OrderProcessingSystem.Models;

public class Order
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string CustomerName { get; set; }

    [Required]
    public string ProductName { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string Status { get; set; }

    public DateTime CreatedDate { get; set; }
}