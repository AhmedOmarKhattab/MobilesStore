using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OnlineShop.Dtos;
using OnlineShop.Enums;
using OnlineShop.Models;

namespace OnlineShop.Services
{
     public  interface IPaymentService
    {
        public Task<string> CreatePaymentSession(Order order);
        public Task<Order> UpdateOrderStatus(string sessionId, OrderStatus status);

    }
}
