using System.ComponentModel.DataAnnotations;

namespace Yetai_Eats.Model
{
    public class IndividualMenuItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Item name is required")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; }

        public double ItemRating { get; set; }

        [Required(ErrorMessage = "Time is required")]
        public string Time { get; set; }

        public string BusinessName { get; set; }
        public string? Email { get; set; }
        public double StoreRating { get; set; }
        public string StoreProfile { get; set; }

        public string Category { get; set; }
    }

}


