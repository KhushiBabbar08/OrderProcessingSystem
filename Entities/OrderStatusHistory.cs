using System;
using System.Collections.Generic;

namespace OrderProcessingSystem.Entities;

public partial class OrderStatusHistory
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public string? OldStatus { get; set; }

    public string NewStatus { get; set; } = null!;

    public DateTime ChangedAt { get; set; }

    public string ChangedBy { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
