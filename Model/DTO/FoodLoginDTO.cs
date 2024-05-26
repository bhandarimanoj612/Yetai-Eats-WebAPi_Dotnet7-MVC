
using System.ComponentModel.DataAnnotations;

namespace Yetai_Eats.Model.DTO
{
    public class FoodLoginDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
