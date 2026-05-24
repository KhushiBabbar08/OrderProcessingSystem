using OrderProcessingSystem.Entities;

namespace OrderProcessingSystem.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetAllAsync();

    Task<Order?> GetByIdAsync(Guid id);

    Task<Order> CreateAsync(Order order);

    Task UpdateAsync(Order order);

    Task DeleteAsync(Order order);
}