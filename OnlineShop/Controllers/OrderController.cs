using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Enums;
using OnlineShop.Helpers;
using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private ApplicationDbContext _context;
        private readonly IPaymentService _paymentService;

        public OrderController(ApplicationDbContext context,IPaymentService paymentService)
        {
            this._context = context;
            this._paymentService = paymentService;
        }
        public async Task<IActionResult> Index()
        {
            var orders =await _context.orders.OrderByDescending(c=>c.Id).ToListAsync();
            return View(orders);
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            List<Product> products = HttpContext.Session
                .Get<List<Product>>("products");
            if(products == null || products.Count == 0)
                return View("Home","Cart");
                foreach(var product in products)
                {
                    OrderItem item = new OrderItem();
                    item.PorductId = product.Id;
                    item.ProductName=product.Name;  
                    item.Price = product.Price*product.Quantity;
                    item.Quantity = product.Quantity;
                    //order.OrderDetails = new List<OrderDetails>();
                    order.Items.Add(item);
                }
                await UpdateProductQuantity(products);
            order.OrderNo = GetOrderNo();
            order.Status=OrderStatus.Pending;
            order.UserName = User.Identity.Name;
           
           await _context.orders.AddAsync(order);



            await _context.SaveChangesAsync();
         var stripeUrl=  await _paymentService.CreatePaymentSession(order);
            HttpContext.Session.Set("products",new List<Product>());
          return Redirect(stripeUrl);
        }
        public async Task UpdateProductQuantity(List<Product> products)
        {
            foreach (var product in products)
            {
                var product2=await _context.Products.FindAsync(product.Id);
                if (product2 == null)
                    continue;
                product2.Quantity -= product.Quantity;
                _context.Update(product2);
            }
            await _context.SaveChangesAsync();
        }


        public string GetOrderNo()
        {
            int rowCount = _context.orders.ToList().Count() + 1;//Orders.ToList().Count() + 1;
            return rowCount.ToString("000");
        }
        public async Task<IActionResult> GetDetails(int id)
        {
            var order =await _context.orders.Where(c=>c.Id==id)
                .Include(c=>c.Items).FirstOrDefaultAsync();
            return View(order); 
        }



    }
}
