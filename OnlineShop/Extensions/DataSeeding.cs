using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Extensions;
using OnlineShop.Models;

namespace BrightMinds.Api.Extensions
{
    public static class DataSeeding
    {
        public static async Task<WebApplication> SeedAppData(this WebApplication application, ApplicationDbContext context)
        {
           if(!context.specialTags.Any())
            {
                var specialTags = new List<SpecialTag>
{
    new SpecialTag
    {
        Name = "جديد"
    },
    new SpecialTag
    {
        Name = "الأكثر مبيعًا"
    },
    new SpecialTag
    {
        Name = "عرض خاص"
    }
};
                await context.AddRangeAsync(specialTags);
                await context.SaveChangesAsync();   


            }
            if (!context.ProductTypes.Any()) {
                var productTypes = new List<ProductType>
{
    new ProductType
    {
        Name = "هواتف ذكية"
    },
    new ProductType
    {
        Name = "إكسسوارات"
    }
};
           


                await context.AddRangeAsync(productTypes);
                await context.SaveChangesAsync();

            }
          if(!context.productBrands.Any())
            {
                var brands = new List<ProductBrand>()
                {
                    new ProductBrand()
                {
                    Name="Samsung"
                },
                    new ProductBrand()
                {
                    Name="IPhone"
                },
                    new ProductBrand()
                {
                    Name="Oppo "
                }
                };

                await context.AddRangeAsync(brands);    
                await context.SaveChangesAsync();
            }

                if (!context.Products.Any())
                {
                    var tagIds = await context.specialTags
                        .Select(t => t.Id)
                        .ToListAsync();

                var brandIds = await context.specialTags
                        .Select(t => t.Id)
                        .ToListAsync();


                var typeIds = await context.ProductTypes
                        .Select(t => t.Id)
                        .ToListAsync();

                    var random = new Random();

                    var products = new List<Product>
    {
        new Product
        {
            Name = "iPhone 15 Pro Max",
            Price = 1199,
            Image = "iphone15promax.webp",
            ProductColor = "تيتانيوم أسود",
            IsAvailable = true,
            ProductTypeId = typeIds[random.Next(typeIds.Count)],
            ProductBrandId = brandIds[random.Next(typeIds.Count)],

            SpecialTagId = tagIds[random.Next(tagIds.Count)],
            Description = "هاتف آبل الرائد بشاشة 6.7 بوصة ومعالج A17 Pro.",
            Quantity = 20
        },

        new Product
        {
            Name = "iPhone 15 Pro Max",
            Price = 1199,
            Image = "iphone15promax.webp",
            ProductColor = "تيتانيوم أسود",
            IsAvailable = true,
            ProductTypeId = typeIds[random.Next(typeIds.Count)],
            ProductBrandId = brandIds[random.Next(typeIds.Count)],

            SpecialTagId = tagIds[random.Next(tagIds.Count)],
            Description = "هاتف آبل الرائد بشاشة 6.7 بوصة ومعالج A17 Pro.",
            Quantity = 20
        },
new Product
        {
            Name = "iPhone 15 Pro Max",
            Price = 1199,
            Image = "iphone15promax.webp",
            ProductColor = "تيتانيوم أسود",
            IsAvailable = true,
            ProductTypeId = typeIds[random.Next(typeIds.Count)],
            ProductBrandId = brandIds[random.Next(typeIds.Count)],

            SpecialTagId = tagIds[random.Next(tagIds.Count)],
            Description = "هاتف آبل الرائد بشاشة 6.7 بوصة ومعالج A17 Pro.",
            Quantity = 20
        },
new Product
        {
            Name = "iPhone 16 Pro Max",
            Price = 1499,
            Image = "iphone15promax.webp",
            ProductColor = "تيتانيوم أسود",
            IsAvailable = true,
            ProductTypeId = typeIds[random.Next(typeIds.Count)],
            ProductBrandId = brandIds[random.Next(typeIds.Count)],

            SpecialTagId = tagIds[random.Next(tagIds.Count)],
            Description = "هاتف آبل الرائد بشاشة 6.7 بوصة ومعالج A17 Pro.",
            Quantity = 20
        },




        new Product
        {
            Name = "Samsung Galaxy A26",
            Price = 1299,
            Image = "s24ultra.jpg",
            ProductColor = "رمادي تيتانيوم",
            IsAvailable = true,
            ProductTypeId = typeIds[random.Next(typeIds.Count)],
            SpecialTagId = tagIds[random.Next(tagIds.Count)],
            ProductBrandId = brandIds[random.Next(typeIds.Count)],

            Description = "هاتف سامسونج مع قلم S-Pen وكاميرا 200 ميجابكسل.",
            Quantity = 15
        },
        new Product
        {
            Name = "Oppo Reno 15 ",
            Price = 1199,
            Image = "oppo.webp",
            ProductColor = "تيتانيوم أسود",
            IsAvailable = true,
            ProductTypeId = typeIds[random.Next(typeIds.Count)],
            ProductBrandId = brandIds[random.Next(typeIds.Count)],

            SpecialTagId = tagIds[random.Next(tagIds.Count)],
            Description = "هاتف  الرائد بشاشة 6.7 بوصة ومعالج A17 Pro.",
            Quantity = 20
        },


        new Product
        {
            Name = "Xiaomi 14 Pro",
            Price = 899,
            Image = "xiaomi14pro.jpg",
            ProductColor = "أسود",
            IsAvailable = true,
            ProductTypeId = typeIds[random.Next(typeIds.Count)],
            SpecialTagId = tagIds[random.Next(tagIds.Count)],
            ProductBrandId = brandIds[random.Next(typeIds.Count)],

            Description = "هاتف شاومي الرائد بكاميرات Leica.",
            Quantity = 18
        },

        new Product
        {
            Name = "Google Pixel 8 Pro",
            Price = 999,
            Image = "pixel8pro.jpg",
            ProductColor = "أزرق سماوي",
            IsAvailable = true,
            ProductTypeId = typeIds[random.Next(typeIds.Count)],
            SpecialTagId = tagIds[random.Next(tagIds.Count)],
            ProductBrandId = brandIds[random.Next(typeIds.Count)],

            Description = "أفضل تجربة أندرويد خالصة.",
            Quantity = 12
        },
        new Product
        {
            Name = "iPhone 12 Pro",
            Price = 1000,
            Image = "iphone15promax.webp",
            ProductColor = "تيتانيوم أسود",
            IsAvailable = true,
            ProductTypeId = typeIds[random.Next(typeIds.Count)],
            ProductBrandId = brandIds[random.Next(typeIds.Count)],

            SpecialTagId = tagIds[random.Next(tagIds.Count)],
            Description = "هاتف آبل الرائد بشاشة 6.7 بوصة ومعالج A17 Pro.",
            Quantity = 20
        },

        new Product
        {
            Name = "iPhone 15 Pro Max",
            Price = 1199,
            Image = "iphone15promax.webp",
            ProductColor = "تيتانيوم أسود",
            IsAvailable = true,
            ProductTypeId = typeIds[random.Next(typeIds.Count)],
            ProductBrandId = brandIds[random.Next(typeIds.Count)],

            SpecialTagId = tagIds[random.Next(tagIds.Count)],
            Description = "هاتف آبل الرائد بشاشة 6.7 بوصة ومعالج A17 Pro.",
            Quantity = 20
        },


        new Product
        {
            Name = "OnePlus 12",
            Price = 799,
            Image = "oneplus12.webp",
            ProductColor = "أخضر زمردي",
            IsAvailable = true,
            ProductTypeId = typeIds[random.Next(typeIds.Count)],
            SpecialTagId = tagIds[random.Next(tagIds.Count)],
            ProductBrandId = brandIds[random.Next(typeIds.Count)],

            Description = "أداء فائق وشحن سريع.",
            Quantity = 17
        },
            new Product
    {
        Name = "Huawei Nova 11",
        Price = 480,
        Image = "nova11.jpg",
        ProductColor = "أخضر",
        IsAvailable = true,
        ProductTypeId = 1,
        SpecialTagId = 3,
        Description = "تصميم أنيق، شاشة OLED وكاميرا أمامية قوية.",
        ProductBrandId = brandIds[random.Next(typeIds.Count)],

        Quantity = 25
    },

    };

                    context.Products.AddRange(products);
                    await context.SaveChangesAsync();
                }



            






            return application;
        }
    }


  


}
