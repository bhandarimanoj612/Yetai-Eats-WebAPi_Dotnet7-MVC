using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Yetai_Eats.Data;
using Yetai_Eats.Model;
using Yetai_Eats.Model.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Yetai_Eats.Services;
using Yetai_Eats.Utils;
using Microsoft.EntityFrameworkCore;

namespace Yetai_Eats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        private readonly ApplicationDbContext _context;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration,IEmailService emailService, IFileService fileService, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailService = emailService;
            _fileService = fileService;
            _context = context;
        }


        [HttpPost("food-register")]
        public async Task<IActionResult> FoodRegister(FoodRegisterDTO model)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    return BadRequest("Email already exists.");
                }

                var newUser = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.Phonenumber
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return BadRequest(errors);
                }

                // Assign role specified in the model
                if (!string.IsNullOrEmpty(model.Role))
                {
                    if (await _userManager.AddToRoleAsync(newUser, model.Role) != IdentityResult.Success)
                    {
                        return BadRequest("Failed to assign role to user.");
                    }
                }

               
               
                if (model.Role == "Seller" || model.Role == "Individual Seller")
                {
                    try
                    {
                        var mailRequest = new MailRequest();
                        EmailHtmlContent emailHtmlContent = new EmailHtmlContent();
                        mailRequest.ToEmail = model.Email;
                        mailRequest.Subject = "Welcome to Yetai Food";
                        mailRequest.Body = emailHtmlContent.AccountRegistrationResponse(model.Username);
                        await _emailService.SendEmailAsync(mailRequest);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else if(model.Role == "Delivery Rider")
                {
                    try
                    {
                        var mailRequest = new MailRequest();
                        EmailHtmlContentRider emailHtmlContent = new EmailHtmlContentRider();
                        mailRequest.ToEmail = model.Email;
                        mailRequest.Subject = "Welcome to Yetai Food";
                        mailRequest.Body = emailHtmlContent.AccountRegistrationResponse(model.Username);
                        await _emailService.SendEmailAsync(mailRequest);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                }
                else if(model.Role == "Customer")
                {
                    try
                    {
                        var mailRequest = new MailRequest();
                        EmailHtmlContentCustomer emailHtmlContent = new EmailHtmlContentCustomer();
                        mailRequest.ToEmail = model.Email;
                        mailRequest.Subject = "Welcome to Yetai Food";
                        mailRequest.Body = emailHtmlContent.AccountRegistrationResponse(model.Username);
                        await _emailService.SendEmailAsync(mailRequest);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                }
                else
                {
                    return BadRequest("Failed to register ");
                }
                    return Ok("User registered successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while registering user: {ex.Message}");
            }
        }

        [HttpPost("food-login")]
        public async Task<IActionResult> FoodLogin(FoodLoginDTO model)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    if (user != null)
                    {
                        var roles = await _userManager.GetRolesAsync(user);

                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, string.Join(",", roles)),
                        new Claim(ClaimTypes.NameIdentifier, user.Id), // Add user ID claim
                        new Claim("UserProfile", user.UserProfile ?? ""), // Add profile image claim with null check
                        new Claim("PhoneNumber", user.PhoneNumber ?? ""), // Add phone number claim with null check
                        new Claim("Address", user.Address ?? ""), // Add address claim with null check
                            }),
                            Expires = DateTime.UtcNow.AddDays(7),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };

                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var tokenString = tokenHandler.WriteToken(token);

                        return Ok(new { Token = tokenString, Username = user.UserName, Email = user.Email, Roles = roles, UserId = user.Id, UserProfile = user.UserProfile, PhoneNumber = user.PhoneNumber, Address = user.Address });
                    }
                    else
                    {
                        return BadRequest("User not found.");
                    }
                }

                return BadRequest("Invalid username or password.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while logging in: {ex.Message}");
            }
        }





        [HttpPut("change-username/{userId}")]
        public async Task<IActionResult> ChangeUsername(string userId, ChangeUsernameDTO model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return NotFound("User not found.");

                user.UserName = model.NewUsername;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                    return Ok("Username changed successfully.");
                else
                    return BadRequest(result.Errors.Select(e => e.Description));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while changing username: {ex.Message}");
            }
        }


        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return NotFound("User not found.");

                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                    return Ok("User deleted successfully.");
                else
                    return BadRequest(result.Errors.Select(e => e.Description));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while deleting user: {ex.Message}");
            }
        }


        [HttpPut("assign-role/{userId}")]
        public async Task<IActionResult> AssignRole(string userId, AssignRoleDTO model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return NotFound("User not found.");

                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles.ToArray());

                var result = await _userManager.AddToRoleAsync(user, model.Role);

                if (result.Succeeded)
                    return Ok("Role assigned successfully.");
                else
                    return BadRequest(result.Errors.Select(e => e.Description));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while assigning role: {ex.Message}");
            }
        }

        // PUT: api/Account/change-password/{userId}
        [HttpPost("change-passwordWithOldPassword/{userId}")]
        public async Task<IActionResult> ChangeWithOldPassword(string userId, ChangePasswordDTO model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return NotFound("User not found.");

                if (model.NewPassword != model.ConfirmPassword)
                    return BadRequest("New password and confirmation password do not match.");

                // Disable two-factor authentication temporarily
                _userManager.Options.SignIn.RequireConfirmedAccount = false;

                // Change the user's password
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                // Re-enable two-factor authentication
                _userManager.Options.SignIn.RequireConfirmedAccount = true;

                if (result.Succeeded)
                    return Ok("Password changed successfully.");
                else
                    return BadRequest(result.Errors.Select(e => e.Description));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while changing password: {ex.Message}");
            }
        }


       


        //change user profile 
        // PUT: api/Account/change-profile-image/{userId}
        [HttpPut("change-profile-image/{userId}")]
        public async Task<IActionResult> ChangeProfileImage(string userId, IFormFile file)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return NotFound("User not found.");

                // Check if the user already has a profile image
                if (!string.IsNullOrEmpty(user.UserProfile))
                {
                    // Delete the existing profile image
                    var existingImagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images\\Files", user.UserProfile);
                    if (System.IO.File.Exists(existingImagePath))
                        System.IO.File.Delete(existingImagePath);
                }

                // Write the new profile image to the file system
                var filename = await _fileService.WriteFile(file);

                // Update the user's profile image
                user.UserProfile = filename;

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Construct the URL to access the uploaded image
                var imageUrl = $"{filename}";

                return Ok(new { message = "Profile image updated successfully.", imageUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while updating profile image: {ex.Message}");
            }
        }


        //addding user location 
        [HttpPost("add-address")]
        public async Task<IActionResult> AddAddress(string address, string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId); // Get the user by ID
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                user.Address = address; // Update the user's address

                var result = await _userManager.UpdateAsync(user); // Update the user in the database
                if (result.Succeeded)
                {
                    return Ok("Address added successfully.");
                }
                else
                {
                    return BadRequest("Failed to add address.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding address: {ex.Message}");
            }
        }

        [HttpPost("add-phoneNumber")]
        public async Task<IActionResult> AddPhone(string phone, string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId); // Get the user by ID
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                user.PhoneNumber = phone; // Update the user's address

                var result = await _userManager.UpdateAsync(user); // Update the user in the database
                if (result.Succeeded)
                {
                    return Ok("Phone Number added successfully.");
                }
                else
                {
                    return BadRequest("Failed to add Phonenumber");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while adding address: {ex.Message}");
            }
        }



        // GET: api/DeliveryRider/AllUser
        [HttpGet("AllUser")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var userDetails = new List<object>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDetail = new
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles
                };
                userDetails.Add(userDetail);
            }

            return Ok(userDetails);
        }


        //send email by coustumer for help
        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail(string email,string body)
        {
            var mailRequest = new MailRequest
            {
                ToEmail = "yetaieats@gmail.com",
              
                Subject = "I need Help ",
                Body = EmailHtmlHelp.GetHelpEmailBody(email, body)
            };

            await _emailService.SendEmailAsync(mailRequest);
            return Ok("Email sent successfully.");
        }




    }
}
