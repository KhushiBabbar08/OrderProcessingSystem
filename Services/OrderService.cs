using OrderProcessingSystem.DTOs;
using OrderProcessingSystem.Interfaces;
using OrderProcessingSystem.Entities;

namespace OrderProcessingSystem.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Order> CreateAsync(CreateOrderDto dto)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerName = dto.CustomerName,
            ProductName = dto.ProductName,
            Quantity = dto.Quantity,
            Price = dto.Price,
            Status = "Pending",
            CreatedDate = DateTime.UtcNow
        };

        return await _repository.CreateAsync(order);
    }

    public async Task<bool> UpdateStatusAsync(Guid id, string status)
    {
        var order = await _repository.GetByIdAsync(id);

        if (order == null)
            return false;

        order.Status = status;

        await _repository.UpdateAsync(order);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var order = await _repository.GetByIdAsync(id);

        if (order == null)
            return false;

        await _repository.DeleteAsync(order);

        return true;
    }
}