using Mango.Services.OrderAPI.DbContexts;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<AppDbContext> _contextOptions;
        public OrderRepository(DbContextOptions<AppDbContext> contextOptions)
        {
            _contextOptions = contextOptions;
        }

        public async Task<bool> AddOrder(OrderHeader orderHeader)
        {
            await using var _context = new AppDbContext(_contextOptions);
            _context.OrderHeaders.Add(orderHeader);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid)
        {
            await using var _context = new AppDbContext(_contextOptions);
            var orderHeaderFromDb = await _context.OrderHeaders.FirstOrDefaultAsync(u => u.Id == orderHeaderId);
            if(orderHeaderFromDb!= null)
            {
                orderHeaderFromDb.PaymentStatus = paid;
                await _context.SaveChangesAsync();
            }
        }
    }
}
