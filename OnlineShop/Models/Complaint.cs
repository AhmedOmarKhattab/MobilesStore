namespace OnlineShop.Models
{
    public class Complaint
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; }
        public string Status { get; set; } = "قيد الانتظار";
    }
}
