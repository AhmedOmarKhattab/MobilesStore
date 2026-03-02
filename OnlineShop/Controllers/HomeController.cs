using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Helpers;
using OnlineShop.Models;
using Stripe.Events;
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
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext dbContext)
        {
            this._context = dbContext;
        }
		public IActionResult Index(int? page)
		{
            var product = _context.Products.Include(c => c.ProductType).Include(c => c.SpecialTag)
                .ToList().ToPagedList(page ?? 1, 12);
			return View(product);
		}

		[HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            
            var product = _context.Products
                .Include(e => e.ProductType)
                .Include(e => e.SpecialTag)
                .Include(e => e.ProductBrand)

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


            var product = _context.Products
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
        public IActionResult WhoUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> ShowProducts(int? type,int? brand,string? name ,int pageNumber=1)
        {
            var query = _context.Products.AsQueryable();
            if(type.HasValue)
                query=query.Where(c=>c.ProductTypeId==type.Value);
            if (brand.HasValue)
                query = query.Where(c => c.ProductBrandId == brand.Value);
            if(!string.IsNullOrEmpty(name))
                query=query.Where(c=>c.Name.ToLower().Contains(name.ToLower()));
            var products = await query
                .Skip((pageNumber-1)*12)
                .Take(12)
                .Include(C=>C.SpecialTag)
                .Include(C=>C.ProductBrand)
                .ToListAsync();
            var count=await query.CountAsync();
            ViewBag.Brands = await _context.productBrands.ToListAsync();

            ViewBag.Types=await _context.ProductTypes.ToListAsync();

            ViewBag.currentType=type;
            ViewBag.brand = brand;
            ViewBag.name = name;
            ViewBag.pageNumber = pageNumber;
            return View(products);

        }
        public async Task<IActionResult> Compailant()
        {
            return View();
        }
    }
}
