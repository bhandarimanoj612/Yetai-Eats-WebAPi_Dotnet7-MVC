using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yetai_Eats.Data;
using Yetai_Eats.Model;
using Yetai_Eats.Model.DTO;
using Yetai_Eats.Services;
using YetaiEatsAPI.Models;
using YetaiEatsAPI.Models.DTO;

namespace YetaiEatsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public MenuItemController(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MenuItem>), 200)]
        public async Task<IActionResult> GetMenuItems()
        {
            var menuItems = await _context.MenuItems.ToListAsync();
            return Ok(menuItems);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MenuItem), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetMenuItemById(int id)
        {
            var menuItem = await _context.MenuItems.FirstOrDefaultAsync(m => m.Id == id);
            if (menuItem == null)
            {
                return NotFound();
            }
            return Ok(menuItem);
        }

        [HttpPost]
        [ProducesResponseType(typeof(MenuItem), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> AddMenuItem([FromForm] MenuItemCreateDTO menuItemDTO, IFormFile itemImg, IFormFile storeProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var menuItem = new MenuItem
            {
                ItemName = menuItemDTO.ItemName,
                Price = menuItemDTO.Price,
                Image = await _fileService.WriteFile(itemImg),
                Time = menuItemDTO.Time,
                BusinessName = menuItemDTO.BusinessName,
                StoreRating = 0,
                StoreProfile = await _fileService.WriteFile(storeProfile),
                Email = menuItemDTO.Email,
                Category= "MenuItems"
            };

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMenuItemById), new { id = menuItem.Id }, menuItem);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MenuItem), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromForm] MenuItemUpdateDTO menuItemDTO, IFormFile itemImg, IFormFile storeProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingMenuItem = await _context.MenuItems.FirstOrDefaultAsync(m => m.Id == id);
            if (existingMenuItem == null)
            {
                return NotFound();
            }

            existingMenuItem.ItemName = menuItemDTO.ItemName;
            existingMenuItem.Price = menuItemDTO.Price;
            existingMenuItem.Image = await _fileService.WriteFile(itemImg);
            existingMenuItem.Time = menuItemDTO.Time;
            existingMenuItem.BusinessName = menuItemDTO.BusinessName;
            existingMenuItem.StoreRating = 0;
            existingMenuItem.StoreProfile = await _fileService.WriteFile(storeProfile);
            existingMenuItem.Email = menuItemDTO.Email;
            existingMenuItem.Category = "MenuItems";
          
            await _context.SaveChangesAsync();

            return Ok(existingMenuItem);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var menuItem = await _context.MenuItems.FirstOrDefaultAsync(m => m.Id == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMenuItems(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term cannot be empty");
            }

            var matchingItems = await _context.MenuItems
                .Where(m => m.ItemName.ToLower().Contains(searchTerm.ToLower()))
                .ToListAsync();

            return Ok(matchingItems);
        }

    }
}