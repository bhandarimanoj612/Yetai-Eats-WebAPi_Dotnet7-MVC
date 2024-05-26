using System.ComponentModel.DataAnnotations;

namespace Yetai_Eats.Model.DTO
{
    public class RatingDTO
    {
        public string SellerName { get; set; }
        public string UserName { get; set; }
        public string ItemName { get; set; }
        public string Email { get; set; }

        public double ItemRating { get; set; }
        public string Category { get; set; }
    }
}
