using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yetai_Eats.Data;
using Yetai_Eats.Model;
using Yetai_Eats.Model.DTO;
using Yetai_Eats.Services;

namespace Yetai_Eats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndividualMenuItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;
        public IndividualMenuItemController(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IndividualMenuItem>), 200)]
        public async Task<IActionResult> GetIndividualMenuItems()
        {
            var menuItems = await _context.IndividualMenuItems.ToListAsync();
            return Ok(menuItems);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IndividualMenuItem), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetIndividualMenuItemById(int id)
        {
            var menuItem = await _context.IndividualMenuItems.FirstOrDefaultAsync(m => m.Id == id);
            if (menuItem == null)
            {
                return NotFound();
            }
            return Ok(menuItem);
        }
        [HttpPost]
        [ProducesResponseType(typeof(IndividualMenuItem), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddIndividualMenuItem([FromForm] IndividualMenuItemCreateDTO menuItemDTO, IFormFile itemImg, IFormFile storeProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var menuItem = new IndividualMenuItem
            {
                ItemName = menuItemDTO.ItemName,
                Price = menuItemDTO.Price,
                Image = await _fileService.WriteFile(itemImg),
                Time = menuItemDTO.Time,
                BusinessName = menuItemDTO.BusinessName,
                StoreRating = 0,
                StoreProfile = await _fileService.WriteFile(storeProfile),
                Email = menuItemDTO.Email,
                Category= "IndividualMenuItem"
            };

            _context.IndividualMenuItems.Add(menuItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIndividualMenuItemById), new { id = menuItem.Id }, menuItem);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(IndividualMenuItem), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateIndividualMenuItem(int id, [FromForm] IndividualMenuItemUpdateDTO menuItemDTO, IFormFile itemImg, IFormFile storeProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingMenuItem = await _context.IndividualMenuItems.FirstOrDefaultAsync(m => m.Id == id);
            if (existingMenuItem == null)
            {
                return NotFound();
            }

            existingMenuItem.ItemName = menuItemDTO.ItemName;
            existingMenuItem.Price = menuItemDTO.Price;
            existingMenuItem.Image = await _fileService.WriteFile(itemImg);
            existingMenuItem.Time = menuItemDTO.Time;
            existingMenuItem.BusinessName = menuItemDTO.BusinessName;
            existingMenuItem.StoreRating = 0 ;
            existingMenuItem.StoreProfile = await _fileService.WriteFile(storeProfile);
            existingMenuItem.Email = menuItemDTO.Email;
            existingMenuItem.Category = "IndividualMenuItems";


            await _context.SaveChangesAsync();

            return Ok(existingMenuItem);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteIndividualMenuItem(int id)
        {
            var menuItem = await _context.IndividualMenuItems.FirstOrDefaultAsync(m => m.Id == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            _context.IndividualMenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchIndividualMenuItems(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term cannot be empty");
            }

            var matchingItems = await _context.IndividualMenuItems
                .Where(m => m.ItemName.ToLower().Contains(searchTerm.ToLower()))
                .ToListAsync();

            return Ok(matchingItems);
        }




    }
}
