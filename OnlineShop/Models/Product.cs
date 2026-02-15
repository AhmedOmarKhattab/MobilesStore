using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public string? Image { get; set; } 
        [Display(Name = "Product Color")]
        public string ProductColor { get; set; }
        [Required]
        [Display(Name = "Available")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Product Type")]
        [Required]
        public int ProductTypeId { get; set; }
        [ForeignKey("ProductTypeId")]
        public  ProductType? ProductType { get; set; }
        [Display(Name = "Special Tag")]
        [Required]
        public int SpecialTagId { get; set; }
        public string Description { get; set; } =string.Empty;
        public int Quantity { get; set; }   
        [ForeignKey("SpecialTagId")]
        public virtual SpecialTag? SpecialTag { get; set; }
    }
}
