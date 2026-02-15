using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class ProductTypeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        // DI
        public ProductTypeController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var data = _dbContext.ProductTypes.ToList();// GetAll
            return View(data);
        }

        [HttpGet]
        public IActionResult Create() 
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductType productTypes)
        {
            if(ModelState.IsValid)
            {
                await _dbContext.ProductTypes.AddAsync(productTypes);
                await _dbContext.SaveChangesAsync();
                TempData["save"] = "Product Type created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id is null)
                return NotFound();
            var productType = _dbContext.ProductTypes.Find(id);
            if(productType == null)
                return NotFound();
            return View(productType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductType productTypes)
        {
            if (ModelState.IsValid)
            {
                 _dbContext.Update(productTypes);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productTypes);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null)
                return NotFound();
            var productType = _dbContext.ProductTypes.Find(id);
            if (productType == null)
                return NotFound();
            return View(productType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult  Details(ProductType productTypes)
        {
             return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null)
                return NotFound();
            var productType = _dbContext.ProductTypes.Find(id);
            if (productType == null)
                return NotFound();
            return View(productType);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id,ProductType productTypes)
        {
            if (id is null)
                return NotFound();

            if(id != productTypes.Id)
                return NotFound();

            var productType = _dbContext.ProductTypes.Find(id);
            TempData["delete"] = "Product Type Deleted";
            if (productType is null)
                return NotFound();
            
            if (ModelState.IsValid)
            {
                _dbContext.Remove(productType);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }             
            return View(productType);
        }
    }
}
