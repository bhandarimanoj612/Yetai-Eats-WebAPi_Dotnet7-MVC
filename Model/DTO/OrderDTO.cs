namespace Yetai_Eats.Model.DTO
{
    public class OrderDTO
    {
        public string SellerName { get; set; }
        public string UserName { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public string Stripe { get; set; }

        public string? Email { get; set; }
        public double TotalPrice { get; set; }
        public string Category { get; set; }
        public string Address { get; set; }//user address
        public string PhoneNumber { get; set; } //user phone number
    }
}
