using System.ComponentModel.DataAnnotations;

namespace YetaiEatsAPI.Models.DTO
{
    public class MenuItemUpdateDTO
    {
        [Required(ErrorMessage = "Item name is required")]
        public string ItemName { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Time is required")]
        public string Time { get; set; }
        public string BusinessName { get; set; }
        public string? Email { get; set; }//geting seller

    }
}

