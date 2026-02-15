using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]  
		public async Task<IActionResult> Register(RegisterModel register)
		{
            if(ModelState.IsValid)// Server Side Validation 
            {
                var user = new ApplicationUser()
                {
                    UserName = register.Email.Split('@')[0], // Use the part before '@' as the username
                    Email = register.Email,
                    FirstName = " ",
                    LastName = " ",
                    IsAgree = register.IsAgree
                };

                var Result = await _userManager.CreateAsync(user,register.Password);

                if (Result.Succeeded)
                    return RedirectToAction(nameof(Login));

                foreach(var error in Result.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                }
            }
			return View(register);
		}

        public IActionResult Login()
        {
            return View();
        }


		[HttpPost]
		public async Task<IActionResult> Login(LoginModel model)
		{
			if (model == null)
			{
				return BadRequest("Invalid login request.");
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			// Find the user by email
			var user = await _userManager.FindByEmailAsync(model.UserName);
			if (user == null)
			{
				ModelState.AddModelError(string.Empty, "Invalid username or password.");
				return View(model);
			}

			var Name = User.Identity.Name;
			// Check if the password matches
			var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
			if (!isPasswordValid)
			{
				ModelState.AddModelError(string.Empty, "Invalid username or password.");
				return View(model);
			}

			// Perform sign-in
			var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
			if (result.Succeeded)
			{
				ViewBag.Name = User.Identity.Name;
				return RedirectToAction("Index", "Home");
			}

			// Handle other potential sign-in results
			if (result.IsLockedOut)
			{
				ModelState.AddModelError(string.Empty, "This account is locked out.");
			}
			else if (result.IsNotAllowed)
			{
				ModelState.AddModelError(string.Empty, "Sign-in is not allowed for this account.");
			}
			else
			{
				ModelState.AddModelError(string.Empty, "Failed to log in. Please try again.");
			}

			return View(model);
		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

         
        public async Task<IActionResult> AssignRoleToUser()
        {
            ViewBag.Users = await _userManager.Users
                .Select(p => new {p.Id, p.UserName}).ToListAsync();

            ViewBag.Roles = new List<string> { "Admin", "User" };

            // note 
            if(!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = "User" });
                await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin", });
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignRoleToUser( string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // Assign role
                var result = await _userManager.AddToRoleAsync(user, role);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Role assigned successfully!";
                    return RedirectToAction(nameof(AssignRoleToUser));
                }
                else
                {
                    TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
                }
            }
            else
            {
                TempData["Error"] = "User not found!";
            }
            return RedirectToAction(nameof(AssignRoleToUser));
        }

    }
}
