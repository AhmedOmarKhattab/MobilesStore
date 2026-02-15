
using BrightMinds.Api.Extensions;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Data;
using OnlineShop.Models;
using System.Runtime.CompilerServices;


namespace OnlineShop.Extensions
{
    public static class UpdateDatabaseExtension
    {
        public async static Task<WebApplication>UpdateDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            await ApplyMigration.ApplyMigrationsAsync(scope);
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
           var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

           await app.SeedUsersAndRoles(userManager, roleManager);
               await app.SeedAppData(context);
            return app;
        }
    }
}
