

using Microsoft.AspNetCore.Identity;
using OnlineShop.Models;
using System.Runtime.CompilerServices;

namespace OnlineShop.Extensions
{
    public static class AppUsersAndRolesSeeding
    {
        public static async Task<WebApplication> SeedUsersAndRoles(this WebApplication application, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                var role = new IdentityRole()
                {
                    Name = "Admin"
                };
               
                await roleManager.CreateAsync(role);

            }


            if (!userManager.Users.Any())
            {

             
                var user = new ApplicationUser()
                {
                    UserName = "ahmmmedomarkhtab",
                    FirstName = "Ahmed",
                    LastName = "Omar Khattab",
                    Email = "ahmmmedomarkhtab@gmail.com",
                    PhoneNumber = "01006370511"
                };


                var result = await userManager.CreateAsync(user, "Password123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                    //var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    //await userManager.ConfirmEmailAsync(user, token);
                }
               



            }
            return application;
        }
    }
}
