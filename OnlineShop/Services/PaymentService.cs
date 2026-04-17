
using OnlineShop.Data;
using OnlineShop.Dtos;
using OnlineShop.Models;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using static System.Net.WebRequestMethods;
using OnlineShop.Enums;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _http;

        public PaymentService(ApplicationDbContext context, IHttpContextAccessor http)
        {
            //this._unitOfWork = unitOfWork;
            //this._orderService = orderService;
            this._context = context;
            this._http = http;
        }
        public async Task<string> CreatePaymentSession(Order order)
        {
           using var transaction= await _context.Database.BeginTransactionAsync();

            try
            {
               // var domain = "http://localhost:5035/api/Course"; // Your frontend URL
                var lineItems = order.Items.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100), // in cents
                        Currency = "usd",
                       
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductName,
                            Description=$"Price: {item.Price}"
                        },
                    },
                    Quantity = item.Quantity,
                }).ToList();
                var request = _http.HttpContext?.Request
                         ?? throw new InvalidOperationException("No active HTTP context");

                // 2️⃣ Build the absolute domain (e.g., https://example.com)
                var domain = $"{request.Scheme}://{request.Host}";
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = lineItems,
                    Mode = "payment",

                    // SuccessUrl = $"{domain}/success?session_id={{CHECKOUT_SESSION_ID}}",
                     SuccessUrl = $"{domain}/Payment/Success?session_id={{CHECKOUT_SESSION_ID}}",

                    CancelUrl = $"{domain}/Payment/Failed?session_id={{CHECKOUT_SESSION_ID}}",
                };

                var service = new SessionService();
                Session session = service.Create(options);
                order.PaymentSessionId = session.Id;
                _context.Update(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return session.Url;
            }
            catch (Exception ex) {

               await transaction.RollbackAsync();
                return String.Empty;
             
            }

        }
        public async Task<Order> UpdateOrderStatus(string sessionId, OrderStatus status)
        {
            var order = await _context.orders.Where(c=>c.PaymentSessionId==sessionId)
                .FirstOrDefaultAsync();
            if (order == null)
                return null;
            _context.Update(order);
            var userId = _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
             await _context.SaveChangesAsync();
            return order;
        }
    }
}
