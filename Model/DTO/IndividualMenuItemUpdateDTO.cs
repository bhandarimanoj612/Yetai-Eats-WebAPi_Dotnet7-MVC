using System.ComponentModel.DataAnnotations;

namespace Yetai_Eats.Model.DTO
{
    public class IndividualMenuItemUpdateDTO
    {
        [Required(ErrorMessage = "Item name is required")]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public double Price { get; set; }



        [Required(ErrorMessage = "Time is required")]
        public string Time { get; set; }

        public string BusinessName { get; set; }
        public string? Email { get; set; }

    }
}
