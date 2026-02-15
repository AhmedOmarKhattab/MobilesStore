using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class ProductType
    {
        public int Id { get; set; }
        [Required]
  
        public string Name { get; set; }
    }
}
