// DiliveryRIder.cs

using System;
using System.ComponentModel.DataAnnotations;

namespace Yetai_Eats.Model
{
    public class DiliveryRIder
    {
        [Key]
        public int Id { get; set; }

        public string SellerName { get; set; }
        public string CustomerName { get; set; }
        public string RiderName { get; set; }

        public int OrderId { get; set; }//order id 
        public string OrderDetails { get; set; }
        public string DiliveryStatus { get; set; }
        public DateTime DiliveredTime { get; set; } = DateTime.Now;
    }
}
