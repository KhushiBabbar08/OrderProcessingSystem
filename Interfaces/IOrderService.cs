using OrderProcessingSystem.DTOs;
using OrderProcessingSystem.Entities;

namespace OrderProcessingSystem.Interfaces;

public interface IOrderService
{
    Task<List<Order>> GetAllAsync();

    Task<Order?> GetByIdAsync(Guid id);

    Task<Order> CreateAsync(CreateOrderDto dto);

    Task<bool> UpdateStatusAsync(Guid id, string status);

    Task<bool> DeleteAsync(Guid id);
}