using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class SpecialTagController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        //DI
        public SpecialTagController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View(_dbContext.specialTags.ToList()); // GetAll
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialTag specialTag)
        {
            if(ModelState.IsValid)
            {
                await _dbContext.specialTags.AddAsync(specialTag);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specialTag);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null)
                return NotFound ();

            var specialTag = _dbContext.specialTags.Find(id);
            if(specialTag == null)
                return NotFound ();

            return View(specialTag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SpecialTag specialTag)
        {
            if (ModelState.IsValid)
            {
                _dbContext.specialTags.Update(specialTag);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specialTag);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var specialTag = _dbContext.specialTags.Find(id);
            if (specialTag == null)
                return NotFound();

            return View(specialTag);
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
            var specialTag = _dbContext.specialTags.Find(id);
            if(specialTag == null)
                return NotFound();

            return View(specialTag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id,SpecialTag specialTags)
        {
            if (id == null)
                return NotFound();

            if (id != specialTags.Id)
                return NotFound();

            var specialTag = _dbContext.specialTags.Find(id);
            if (specialTag == null)
                return NotFound();

            if(ModelState.IsValid)
            {
                _dbContext.Remove(specialTag);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof (Index));
            }
            return View(specialTag);
        }
    }
}
