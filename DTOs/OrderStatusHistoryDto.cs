namespace OrderProcessingSystem.DTOs
{
    public class OrderStatusHistoryDto
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public string? OldStatus { get; set; }

        public string NewStatus { get; set; } = string.Empty;

        public DateTime ChangedAt { get; set; }

        public string ChangedBy { get; set; } = string.Empty;

        public string? Remarks { get; set; }

        public string? Source { get; set; }
    }
}
