//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Yetai_Eats.Data;
//using Yetai_Eats.Model;
//using Yetai_Eats.Model.DTO;

//namespace Yetai_Eats.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CartController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;

//        public CartController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        [HttpGet("{userId}")]
//        public async Task<IActionResult> GetCartItems(string userId)
//        {
//            var cartItems = await _context.CartItems.Where(c => c.UserId == userId).ToListAsync();
//            return Ok(cartItems);
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddToCart(CartItemDTO cartItemDTO)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            // Retrieve the menu item from the database based on the item ID
//            var menuItem = await _context.MenuItems.FindAsync(cartItemDTO.ItemId);
//            if (menuItem == null)
//            {
//                return NotFound("Menu item not found");
//            }

//            // Calculate the total price based on the quantity of the item added
//            double totalPrice = menuItem.Price * cartItemDTO.Quantity;

//            // Create a new CartItem instance with the calculated total price
//            var cartItem = new CartItem
//            {
//                UserId = cartItemDTO.UserId,
//                ItemId = cartItemDTO.ItemId,
//                Quantity = cartItemDTO.Quantity,
//                TotalPrice = totalPrice
//            };

//            // Add the cart item to the database
//            _context.CartItems.Add(cartItem);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetCartItems), new { userId = cartItem.UserId }, cartItem);
//        }


//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateCartItem(int id, CartItem updatedCartItem)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            var existingCartItem = await _context.CartItems.FindAsync(id);
//            if (existingCartItem == null)
//            {
//                return NotFound();
//            }

//            existingCartItem.Quantity = updatedCartItem.Quantity;

//            await _context.SaveChangesAsync();

//            return Ok(existingCartItem);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> RemoveFromCart(int id)
//        {
//            var cartItem = await _context.CartItems.FindAsync(id);
//            if (cartItem == null)
//            {
//                return NotFound();
//            }

//            _context.CartItems.Remove(cartItem);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }
//    }
//}
