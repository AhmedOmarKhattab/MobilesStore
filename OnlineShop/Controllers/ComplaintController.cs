using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using System.Threading.Tasks;

namespace OnlineShop.Controllers
{
    public class ComplaintController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ComplaintController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _dbContext.complaints.ToListAsync();
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult>ChangeStatus(int id)
        {
            var comp=await _dbContext.complaints.FindAsync(id);
            if(comp==null)
                return View("Error");
            comp.Status = "تم الرد";
           await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult>Create(Complaint complaint)
        {
            _dbContext.Add(complaint);
           await _dbContext.SaveChangesAsync();

            TempData["Success"] = "تم إرسال الشكوى بنجاح ✅";

            return RedirectToAction("Index", "Home");
        }


    }
}
