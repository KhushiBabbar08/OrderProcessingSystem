using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.Data;
using OrderProcessingSystem.DTOs;
using OrderProcessingSystem.Entities;
using OrderProcessingSystem.Interfaces;

namespace OrderProcessingSystem.Services
{
    public class OrderHistoryService : IOrderHistoryService
    {
        private readonly OrderProcessingDbContext _context;

        public OrderHistoryService(OrderProcessingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderStatusHistoryDto>> GetHistoryAsync(Guid orderId)
        {
            var history = await _context.OrderStatusHistories
                .Where(x => x.OrderId == orderId)
                .OrderByDescending(x => x.ChangedAt)
                .Select(x => new OrderStatusHistoryDto
                {
                    Id = x.Id,
                    OrderId = x.OrderId,
                    OldStatus = x.OldStatus,
                    NewStatus = x.NewStatus,
                    ChangedAt = x.ChangedAt,
                    ChangedBy = x.ChangedBy,
                    //Remarks = x.Remarks,
                    //Source = x.Source
                })
                .ToListAsync();

            return history;
        }

        public async Task AddHistoryAsync(
            Guid orderId,
            string? oldStatus,
            string newStatus,
            string changedBy,
            string? remarks = null,
            string? source = null)
        {
            var history = new OrderStatusHistory
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = changedBy,
                //Remarks = remarks,
                //Source = source
            };

            await _context.OrderStatusHistories.AddAsync(history);

            await _context.SaveChangesAsync();
        }
    }
}