using System.ComponentModel.DataAnnotations;

namespace Yetai_Eats.Model
{
    public class Rating
    {
        public int Id { get; set; }

        [Required]
        public string SellerName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public double ItemRating { get; set; }

        public string Category { get; set; }
    }
}
