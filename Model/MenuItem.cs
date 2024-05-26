using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Yetai_Eats.Model
{
    public class MenuItem
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

        //for user side 
        public string Time { get; set; }//time for eadch items 
        public string BusinessName { get; set; }//this is for businessname

        public string? Email { get; set; }//geting seller

        public double StoreRating { get; set; }//this is for store rating 
        public string StoreProfile { get; set; }//this is for profile image

        public string Category { get; set; }
    }

}
