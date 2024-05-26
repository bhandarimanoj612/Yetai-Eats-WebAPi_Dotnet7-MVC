using System.ComponentModel.DataAnnotations;
using Yetai_Eats.Model.DTO;

namespace Yetai_Eats.Model
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string SellerName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public DateTime OrderTime { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string Stripe { get; set; }
        public string? Email { get; set; }

        public string Address { get; set; } //customer address

        public string PhoneNumber { get; set; } //Customer Phone Number 


        public double TotalPrice { get; set; }

        public string Category { get; set; }

        // New fields for delivery functionality
        public string DeliveryStatus { get; set; } // Pending, In Progress, Delivered
    }
}
