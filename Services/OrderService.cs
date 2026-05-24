using OrderProcessingSystem.DTOs;
using OrderProcessingSystem.Interfaces;
using OrderProcessingSystem.Entities;
using System.Diagnostics;

namespace OrderProcessingSystem.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly ICacheService _cacheService;

    public OrderService(
        IOrderRepository repository,
        ICacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        var stopwatch = Stopwatch.StartNew();

        const string cacheKey = "orders:all";

        var cachedOrders =
            await _cacheService
                .GetDataAsync<List<Order>>(cacheKey);

        if (cachedOrders != null)
        {
            stopwatch.Stop();

            Console.WriteLine(
                $"Fetched From Redis in {stopwatch.ElapsedMilliseconds} ms");

            return cachedOrders;
        }

        var orders = await _repository.GetAllAsync();

        await _cacheService.SetDataAsync(
            cacheKey,
            orders,
            TimeSpan.FromMinutes(5));

        stopwatch.Stop();

        Console.WriteLine(
            $"Fetched From SQL Server in {stopwatch.ElapsedMilliseconds} ms");

        return orders;
    }
    public async Task<Order?> GetByIdAsync(Guid id)
    {
        var cacheKey = $"orders:{id}";

        var cachedOrder =
            await _cacheService
                .GetDataAsync<Order>(cacheKey);

        if (cachedOrder != null)
        {
            Console.WriteLine("Fetched Single Order From Redis");

            return cachedOrder;
        }

        Console.WriteLine("Fetched Single Order From SQL Server");

        var order = await _repository.GetByIdAsync(id);

        if (order != null)
        {
            await _cacheService.SetDataAsync(
                cacheKey,
                order,
                TimeSpan.FromMinutes(5));
        }

        return order;
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

        var createdOrder =
            await _repository.CreateAsync(order);

        // Invalidate orders list cache
        await _cacheService.RemoveDataAsync("orders:all");

        // Add newly created order into cache
        await _cacheService.SetDataAsync(
            $"orders:{createdOrder.Id}",
            createdOrder,
            TimeSpan.FromMinutes(5));

        return createdOrder;
    }

    public async Task<bool> UpdateStatusAsync(Guid id, string status)
    {
        var order = await _repository.GetByIdAsync(id);

        if (order == null)
            return false;

        order.Status = status;

        await _repository.UpdateAsync(order);

        // Remove stale caches
        await _cacheService.RemoveDataAsync("orders:all");

        await _cacheService.RemoveDataAsync(
            $"orders:{id}");

        // Add updated order back into cache
        await _cacheService.SetDataAsync(
            $"orders:{id}",
            order,
            TimeSpan.FromMinutes(5));

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var order = await _repository.GetByIdAsync(id);

        if (order == null)
            return false;

        await _repository.DeleteAsync(order);

        // Remove caches
        await _cacheService.RemoveDataAsync("orders:all");

        await _cacheService.RemoveDataAsync(
            $"orders:{id}");

        return true;
    }
}