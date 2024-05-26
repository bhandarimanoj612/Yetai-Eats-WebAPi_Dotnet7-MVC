using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Yetai_Eats.Model
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        [MinLength(10)]
        [MaxLength(10)]
        public override string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public string? UserProfile { get; set; }
      
    }
}
