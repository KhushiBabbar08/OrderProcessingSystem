using OrderProcessingSystem.DTOs;

namespace OrderProcessingSystem.Interfaces
{
    public interface IOrderHistoryService
    {
        Task<IEnumerable<OrderStatusHistoryDto>> GetHistoryAsync(Guid orderId);
        Task AddHistoryAsync(
            Guid orderId,
            string? oldStatus,
            string newStatus,
            string changedBy,
            string? remarks = null,
            string? source = null);
    }
}
