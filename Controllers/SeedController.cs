using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Yetai_Eats.Data;
using Yetai_Eats.Model;
using Yetai_Eats.Model.DTO;

namespace Yetai_Eats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
     

        public SeedController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
          
        }


        [HttpPost("seed-users")]
        public async Task<IActionResult> SeedUsers()
        {
            try
            {
                string[] roleNames = { "Admin", "Seller", "Customer", "Delivery Rider", "Individual Seller" };
                foreach (var roleName in roleNames)
                {
                    if (!await _roleManager.RoleExistsAsync(roleName))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                var adminUser = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    PhoneNumber = "1234567890"
                };

                var adminPassword = "Admin@123";

                if (await _userManager.FindByEmailAsync(adminUser.Email) == null)
                {
                    var result = await _userManager.CreateAsync(adminUser, adminPassword);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }

                return Ok("Users seeded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error seeding users: {ex.Message}");
            }
        }

        private string GetRoleFromSignUpType(string signUpType)
        {
            switch (signUpType)
            {
                case "BuyerSignUp":
                    return "Customer";
                case "BusinessSignUp":
                    return "Seller";
                case "IndividualSignUp":
                    return "Individual Seller";
                case "DeliveryRiderSignUp":
                    return "Delivery Rider";
                default:
                    return "Customer";
            }
        }
    }
}
