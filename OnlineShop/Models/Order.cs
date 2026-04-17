using System.ComponentModel.DataAnnotations;
using OnlineShop.Enums;
using Stripe.Climate;

namespace OnlineShop.Models
{
    public class Order
    {
    public Order()
    {
        Items = new List<OrderItem>();
    }
    public int Id { get; set; }
    [Display(Name = "Order No")]
    public string OrderNo { get; set; }
    public string UserName { get; set; }=string.Empty;
    [Required]
    public string Name { get; set; }
    [Required]
    [Display(Name = "Phone No")]
    public string PhoneNo { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Address { get; set; }
    public string Status { set; get; } = "قيد الانتظار";
    public DateTime OrderDate { get; set; }=DateTime.Now;
    public string? PaymentSessionId { set; get; }
    public virtual List<OrderItem> Items { get; set; }
    }
}
