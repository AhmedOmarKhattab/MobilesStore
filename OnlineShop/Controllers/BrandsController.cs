using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class BrandsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        //DI
        public BrandsController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View(_dbContext.productBrands.ToList()); // GetAll
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductBrand  productBrand)
        {
            if(ModelState.IsValid)
            {
                await _dbContext.productBrands.AddAsync(productBrand);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productBrand);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null)
                return NotFound ();

            var specialTag = _dbContext.productBrands.Find(id);
            if(specialTag == null)
                return NotFound ();

            return View(specialTag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductBrand  productBrand)
        {
            if (ModelState.IsValid)
            {
                _dbContext.productBrands.Update(productBrand);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productBrand);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var brand = _dbContext.productBrands.Find(id);
            if (brand == null)
                return NotFound();

            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult  Details(SpecialTag specialTag)
        {
           return RedirectToAction(nameof (Index));
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if(id ==null)
                return NotFound();
            var brand = _dbContext.productBrands.Find(id);
            if(brand == null)
                return NotFound();

            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id,SpecialTag specialTags)
        {
            if (id == null)
                return NotFound();

            if (id != specialTags.Id)
                return NotFound();

            var brand = _dbContext.productBrands.Find(id);
            if (brand == null)
                return NotFound();

            if(ModelState.IsValid)
            {
                _dbContext.Remove(brand);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof (Index));
            }
            return View(brand);
        }
    }
}
