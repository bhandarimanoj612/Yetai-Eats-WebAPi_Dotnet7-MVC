using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Yetai_Eats.Data;
using Yetai_Eats.Model;
using Yetai_Eats.Model.DTO;

namespace Yetai_Eats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RatingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("AllRatings")]
        public IActionResult GetAllRatings()
        {
            try
            {
                var allRatings = _context.Ratings.ToList();
                return Ok(allRatings);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving ratings: {ex.Message}");
            }
        }

        [HttpGet("RatingsByUsername/{username}")]
        public IActionResult GetRatingsByUsername(string username)
        {
            try
            {
                var ratingsByUsername = _context.Ratings.Where(r => r.UserName == username).ToList();
                return Ok(ratingsByUsername);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving ratings by username: {ex.Message}");
            }
        }

        [HttpGet("RatingsByEmail/{email}")]
        public IActionResult GetRatingsByEmail(string email)
        {
            try
            {
                var ratingsByEmail = _context.Ratings.Where(r => r.Email == email).ToList();
                return Ok(ratingsByEmail);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving ratings by email: {ex.Message}");
            }
        }


        [HttpPost("AddRateOrderItem")]
        public IActionResult AddRateOrderItem(RatingDTO ratingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the provided category is valid
            if (string.Compare(ratingDTO.Category, "MenuItems", true) != 0 &&
                string.Compare(ratingDTO.Category, "IndividualMenuItems", true) != 0)
            {
                return BadRequest("Invalid category provided");
            }

            // Check if the rating already exists for the user and item
            var existingRating = _context.Ratings.FirstOrDefault(r =>
                r.UserName == ratingDTO.UserName &&
                r.ItemName == ratingDTO.ItemName);

            if (existingRating != null)
            {
                // Rating already exists, update it
                existingRating.ItemRating = ratingDTO.ItemRating;

                // Update the rating based on the provided category
                if (string.Compare(ratingDTO.Category, "MenuItems", true) == 0)
                {
                    UpdateMenuItemRating(ratingDTO.ItemName);
                }
                else if (string.Compare(ratingDTO.Category, "IndividualMenuItems", true) == 0)
                {
                    UpdateIndividualItemRating(ratingDTO.ItemName);
                }

                _context.SaveChanges();

                return Ok("Rating updated successfully");
            }
            else
            {
                // Rating doesn't exist, create a new one
                var newRating = new Rating
                {
                    SellerName = ratingDTO.SellerName,
                    UserName = ratingDTO.UserName,
                    ItemName = ratingDTO.ItemName,
                    Email = ratingDTO.Email,
                    ItemRating = ratingDTO.ItemRating,
                    Category = ratingDTO.Category
                };

                // Add the new rating to the context
                _context.Ratings.Add(newRating);

                // Update the rating based on the provided category
                if (string.Compare(ratingDTO.Category, "MenuItems", true) == 0)
                {
                    UpdateMenuItemRating(ratingDTO.ItemName);
                }
                else if (string.Compare(ratingDTO.Category, "IndividualMenuItems", true) == 0)
                {
                    UpdateIndividualItemRating(ratingDTO.ItemName);
                }

                _context.SaveChanges();

                return Ok("Rating saved successfully");
            }
        }


        [HttpPut("UpdateRating/{ratingId}")]
        public IActionResult UpdateRating(int ratingId, RatingDTO updatedRating)
        {
            try
            {
                var existingRating = _context.Ratings.Find(ratingId);
                if (existingRating == null)
                {
                    return NotFound($"Rating with ID {ratingId} not found");
                }

                // Update the existing rating
                existingRating.ItemRating = updatedRating.ItemRating;

                _context.SaveChanges();

                return Ok("Rating updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating rating: {ex.Message}");
            }
        }

        [HttpDelete("DeleteRating/{ratingId}")]
        public IActionResult DeleteRating(int ratingId)
        {
            try
            {
                var existingRating = _context.Ratings.Find(ratingId);
                if (existingRating == null)
                {
                    return NotFound($"Rating with ID {ratingId} not found");
                }

                // Remove the existing rating
                _context.Ratings.Remove(existingRating);

                _context.SaveChanges();

                return Ok("Rating deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting rating: {ex.Message}");
            }
        }


        private void UpdateMenuItemRating(string itemName)
        {
            var menuItem = _context.MenuItems.FirstOrDefault(m => m.ItemName == itemName);
            if (menuItem != null)
            {
                var ratings = _context.Ratings.Where(r => r.ItemName == itemName).Select(r => r.ItemRating);
                if (ratings.Any())
                {
                    menuItem.ItemRating = ratings.Average();
                    _context.SaveChanges();
                }
            }
        }

        private void UpdateIndividualItemRating(string itemName)
        {
            var individualMenuItem = _context.IndividualMenuItems.FirstOrDefault(m => m.ItemName == itemName);
            if (individualMenuItem != null)
            {
                var ratings = _context.Ratings.Where(r => r.ItemName == itemName).Select(r => r.ItemRating);
                if (ratings.Any())
                {
                    individualMenuItem.ItemRating = ratings.Average();
                    _context.SaveChanges();
                }
            }
        }
    }
}
