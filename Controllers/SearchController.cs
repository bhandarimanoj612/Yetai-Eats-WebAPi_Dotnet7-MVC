using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yetai_Eats.Data;

namespace Yetai_Eats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMenuItems(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term cannot be empty");
            }

            var menuItems = await _context.MenuItems
                .Where(m => m.ItemName.ToLower().Contains(searchTerm.ToLower()))
                .ToListAsync();

            var individualMenuItems = await _context.IndividualMenuItems
                .Where(m => m.ItemName.ToLower().Contains(searchTerm.ToLower()))
                .ToListAsync();

            var matchingItems = new List<object>();
            matchingItems.AddRange(menuItems);
            matchingItems.AddRange(individualMenuItems);

            return Ok(matchingItems);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularMenuItems()
        {
            var popularMenuItems = await _context.MenuItems
                .OrderByDescending(m => m.ItemRating)
                .Take(5)
                .ToListAsync();

            var popularIndividualMenuItems = await _context.IndividualMenuItems
                .OrderByDescending(m => m.ItemRating)
                .Take(5)
                .ToListAsync();

            var popularItems = new List<object>();
            popularItems.AddRange(popularMenuItems);
            popularItems.AddRange(popularIndividualMenuItems);

            return Ok(popularItems);
        }

        [HttpGet("specific-seller-items")]
        public async Task<IActionResult> GetSpecificSellerItems(string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                return BadRequest("User email cannot be empty");
            }

            var relatedItems = await _context.MenuItems
                .Where(m => m.Email == userEmail)
                .ToListAsync();

            return Ok(relatedItems);
        }

        [HttpGet("business-items")]
        public async Task<IActionResult> GetBusinessItems(string businessName)
        {
            if (string.IsNullOrWhiteSpace(businessName))
            {
                return BadRequest("Business name cannot be empty");
            }

            var menuItems = await _context.MenuItems
                .Where(m => m.BusinessName.ToLower() == businessName.ToLower())
                .ToListAsync();

            var individualMenuItems = await _context.IndividualMenuItems
                .Where(m => m.BusinessName.ToLower() == businessName.ToLower())
                .ToListAsync();

            var businessItems = new List<object>();
            businessItems.AddRange(menuItems);
            businessItems.AddRange(individualMenuItems);

            return Ok(businessItems);
        }



    }
}
