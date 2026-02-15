using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Helpers;
using OnlineShop.Models;
using System.Diagnostics;
using X.PagedList.Extensions;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        private readonly ApplicationDbContext dbContext;
        public HomeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
		public IActionResult Index(int? page)
		{
            var product = dbContext.Products.Include(c => c.ProductType).Include(c => c.SpecialTag)
                .ToList().ToPagedList(page ?? 1, 9);
			return View(product);
		}

		[HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            
            var product = dbContext.Products
                .Include(e => e.ProductType)
                .Include(e => e.SpecialTag)
                .FirstOrDefault(e => e.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddToCart(int? id,int quantity)
        {
            List<Product> products = new List<Product>();

            if (id == null)
                return NotFound();


            var product = dbContext.Products
                .Include(e => e.ProductType)
                .Include(e => e.SpecialTag)
                .FirstOrDefault(e => e.Id == id);
            if (product == null)
                return NotFound();
            if (quantity <= 0 || product.Quantity < quantity)
            {
                ModelState.AddModelError(string.Empty, "Quantity must be greater than 0 and less than available quantity");
                return View("Details",product);
            }
            product.Quantity = quantity;
            products = HttpContext.Session.Get<List<Product>>("products");
            if(products == null)
            {
                products = new List<Product>();
            }
            products.Add(product);
            HttpContext.Session.Set("products", products);
            return View("Details", product);
        }

        [HttpGet]
        [ActionName("Remove")]
        [Authorize]
        public IActionResult RemoveToCart(int? id)
        {
            List<Product> products = HttpContext.Session
                .Get<List<Product>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(e => e.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        public IActionResult Remove(int? id)
        {
            List<Product> products = HttpContext.Session.Get<List<Product>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(e => e.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Cart()
        {
            List<Product> products = HttpContext.Session.Get<List<Product>>("products");
            if (products == null)
            {
                products = new List<Product>();
            }
            return View(products);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
