using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            this._userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, user.PasswordHash);

                if (result.Succeeded)
                {
                    TempData["messege"] = "User created successfully";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            

            return View();
        }
    }
}
