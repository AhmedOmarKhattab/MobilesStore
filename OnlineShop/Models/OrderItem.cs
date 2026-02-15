using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        [Display(Name = "Order")]
        public int OrderId { get; set; }
        [Display(Name = "Product")]
        public int PorductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
    }
}
