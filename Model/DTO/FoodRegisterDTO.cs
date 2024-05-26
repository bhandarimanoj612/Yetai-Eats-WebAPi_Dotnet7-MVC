using System.ComponentModel.DataAnnotations;

namespace Yetai_Eats.Model.DTO
{
    public class FoodRegisterDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must have at least 10 digits")]
        public string Phonenumber { get; set; }

        public string Role { get; set; } // Add role property
    }
}
