using System;
using System.Collections.Generic;

namespace OrderProcessingSystem.Entities;

public partial class Order
{
    public Guid Id { get; set; }

    public string CustomerName { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public string? Status { get; set; }

    public DateTime CreatedDate { get; set; }
}
