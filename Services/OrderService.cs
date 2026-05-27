using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.DTOs;
using OrderProcessingSystem.Entities;
using OrderProcessingSystem.Events;
using OrderProcessingSystem.Interfaces;
using System.Diagnostics;

namespace OrderProcessingSystem.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly ICacheService _cacheService;
    private readonly IKafkaProducerService _kafkaProducer;
    private readonly IOrderHistoryService _historyService;
    public OrderService(
        IOrderRepository repository,
        ICacheService cacheService,
        IKafkaProducerService kafkaProducer,
        IOrderHistoryService historyService)
    {
        _repository = repository;
        _cacheService = cacheService;
        _kafkaProducer = kafkaProducer;
        _historyService = historyService;
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

        await _cacheService.RemoveDataAsync("orders:all");

        await _cacheService.SetDataAsync(
            $"orders:{createdOrder.Id}",
            createdOrder,
            TimeSpan.FromMinutes(5));

        var orderCreatedEvent = new OrderCreatedEvent
        {
            OrderId = createdOrder.Id,
            CustomerName = createdOrder.CustomerName,
            ProductName = createdOrder.ProductName,
            Quantity = createdOrder.Quantity,
            Price = createdOrder.Price,
            Status = createdOrder.Status,
            CreatedDate = createdOrder.CreatedDate
        };

        await _kafkaProducer.PublishOrderCreatedAsync(
            "order-created-topic",
            orderCreatedEvent);

        return createdOrder;
    }
    public async Task<bool> UpdateStatusAsync(Guid id, string status)
    {
        var order = await _repository.GetByIdAsync(id);

        if (order == null)
            return false;

        order.Status = status;
        var oldStatus = order.Status;

        order.Status = status;

        await _historyService.AddHistoryAsync(
            order.Id,
            oldStatus,
            status,
            "System",
            "Order status updated",
            "API");

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