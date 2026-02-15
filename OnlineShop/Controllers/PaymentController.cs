
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Dtos;
using OnlineShop.Enums;
using OnlineShop.Services;
using Stripe;
using Stripe.Checkout;

namespace OnlineShop.Controllers
{
 
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public PaymentController(IPaymentService paymentService,
            IConfiguration configuration,
          ApplicationDbContext context
            )
        {
            this._paymentService = paymentService;
            this._configuration = configuration;
            this._context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Success(string session_id) 
        {
            var order= await _context.orders.Where(c=>c.PaymentSessionId==session_id)
                .Include(c=>c.Items).FirstOrDefaultAsync();
            return View(order);
        }


      
        [HttpPost]
        public async Task<IActionResult> webhook()
        {
            //await _courseService.CreateAsync(new CategoryDto()
            //{
            //   Name="i am form webhook"
            //});
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                  _configuration["StripeSetting:SecretWebHook"]
                );

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Session;
                    var sessionId = session.Id; // ✅ This is the Session ID you stored
                    await  _paymentService.UpdateOrderStatus(sessionId, OrderStatus.Paid);   
                    // TODO: Create order in your database, mark items as paid
                    Console.WriteLine($"PaymentIntent was successful! ID: {session.PaymentIntentId}");
                    return Ok();
                }
                if(stripeEvent.Type== "checkout.session.expired")
            {
                var session = stripeEvent.Data.Object as Session;
                var sessionId = session.Id; // ✅ This is the Session ID you stored
                await _paymentService.UpdateOrderStatus(sessionId, OrderStatus.Failed);
                return BadRequest();
            }
            else 
            {
               
                return BadRequest();
            }
          
               
            
        }

    }
}
