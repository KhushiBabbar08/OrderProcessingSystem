using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.Entities;
using OrderProcessingSystem.Interfaces;

namespace OrderProcessingSystem.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderProcessingDbContext _context;

    public OrderRepository(OrderProcessingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _context.Orders
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Order> CreateAsync(Order order)
    {
        _context.Orders.Add(order);

        await _context.SaveChangesAsync();

        return order;
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Order order)
    {
        _context.Orders.Remove(order);

        await _context.SaveChangesAsync();
    }
}