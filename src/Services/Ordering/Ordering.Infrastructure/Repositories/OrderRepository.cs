using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            /*
            var lst = _dbContext.Orders
                .Select(o => new { o.Id, o.UserName, o.TotalPrice, o.FirstName, o.LastName, o.EmailAddress, o.AddressLine, o.Country, o.State, o.ZipCode,
                    o.CardName, o.CardNumber, o.Expiration, o.CVV, o.PaymentMethod, o.CreatedBy, o.CreatedDate, o.LastModifiedBy, o.LastModifiedDate }).ToList();
            */

            var orderList = await _dbContext.Orders
                                    .Where(o => o.UserName == userName)
                                    .ToListAsync();
            return orderList;
        }
    }
}