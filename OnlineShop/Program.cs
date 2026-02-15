using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using OnlineShop.Extensions;
using OnlineShop.Services;

namespace OnlineShop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("constr")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                //options.SignIn.RequireConfirmedAccount = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
            Stripe.StripeConfiguration.ApiKey=builder.Configuration["StripeSetting:SecretKey"]; // Your secret key

            builder.Services.AddScoped<IPaymentService, PaymentService>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = ".AdventureWorks.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();
            await app.UpdateDatabase();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //app.MapRazorPages();

            app.Run();
        }
    }
}
