using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Helpers;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    // Identity => should user make login ,
    // before controller 
    // before action 
   // [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {

        private   ApplicationDbContext _db;

        // DI
        public ProductController(ApplicationDbContext dbContext)
        {
            this._db = dbContext;
        }
        public IActionResult Index()
        {
            return View(_db.Products.Include(e => e.ProductType)
                .Include(e => e.SpecialTag)
                .ToList());
        }

        [HttpPost]
        public IActionResult Index(decimal? lowAmount, decimal? largeAmount)
        {
            var products = _db.Products.Include(e => e.ProductType)
                .Include(e => e.SpecialTag)
                .Where(e => e.Price >= lowAmount && e.Price <= largeAmount)
                .ToList();
            if(lowAmount == null || largeAmount == null)
            {
                products = _db.Products.Include(e => e.ProductType)
                    .Include(e => e.SpecialTag).ToList();
            }
            return View(products);
        }
        //Get Create method
        //[Authorize]
        public IActionResult Create()
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "Name");
            ViewData["TagId"] = new SelectList(_db.specialTags.ToList(), "Id", "Name");
            return View();
        }

        //Post Create method
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var searchProduct = _db.Products
                    .FirstOrDefault(e => e.Name == product.Name);
                if(searchProduct != null)
                {
                    ViewBag.messege = "This product already exist";
                    ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "Name");
                    ViewData["TagId"] = new SelectList(_db.specialTags.ToList(), "Id", "Name");
                    return View(product);
                }
                    
                
                if (image != null)
                {
                    var fileName = DocumentSettings.UploadImage(image, "images");
                    product.Image = fileName;
                }

                //if (image == null)
                //{
                //    product.Image = "Images/noimage.PNG";
                //}

                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product created successfully!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "Name");
            ViewData["TagId"] = new SelectList(_db.specialTags.ToList(), "Id", "Name");
            return View(product);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            // Fetch the product with the given id
            var product = _db.Products
                             .Include(c => c.ProductType)
                             .Include(c => c.SpecialTag)
                             .FirstOrDefault(c => c.Id == id);

            if (product == null)
                return NotFound();

            // Populate the dropdown lists for ProductType and SpecialTag
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "Name");
            ViewData["TagId"] = new SelectList(_db.specialTags.ToList(), "Id", "Name");

            // Pass the product to the view
            return View(product);
        }


        //Post Create method
        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var fileName = DocumentSettings.UploadImage(image, "images");
                    product.Image = fileName;
                }


                _db.Products.Update(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Populate the dropdown lists again if the model is invalid
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "Name");
            ViewData["TagId"] = new SelectList(_db.specialTags.ToList(), "Id", "Name");
            return View(product);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if(id== null)
                return NotFound();

            var product = _db.Products.Include(e => e.ProductType)
                .Include(e => e.SpecialTag).FirstOrDefault(e => e.Id == id);

            if(product == null)
                return NotFound();
            return View(product);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if(id== null)
                return NotFound();

            var product = _db.Products.Include(e => e.SpecialTag)
                .Include(e => e.ProductType).Where(e => e.Id == id)
                .FirstOrDefault(e => e.Id == id);

            if(product == null)
                return NotFound();
            return View(product);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
                return NotFound();

            var product = _db.Products.FirstOrDefault(e => e.Id==id);
            TempData["delete"] = "Product Deleted";
            if (product == null)
                return NotFound();
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
 