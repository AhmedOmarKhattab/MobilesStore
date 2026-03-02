using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Models;

namespace OnlineShop.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<SpecialTag> specialTags { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderItem> orderDetails { get; set; }
        public DbSet<ProductBrand> productBrands { get; set; }
        //public DbSet<ApplicationUser> applicationUsers { get; set; }

         

    }
}
