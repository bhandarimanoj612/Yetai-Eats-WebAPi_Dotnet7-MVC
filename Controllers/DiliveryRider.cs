// DeliveryRiderController.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yetai_Eats.Data;
using Yetai_Eats.Model;

namespace Yetai_Eats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryRiderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeliveryRiderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/DeliveryRider
        [HttpGet]
        public async Task<IActionResult> GetDeliveryRiders()
        {
            var deliveryRiderRole = await _userManager.GetUsersInRoleAsync("Delivery Rider");
            if (deliveryRiderRole == null)
            {
                return NotFound("Delivery Rider role not found.");
            }

            var deliveryRiderDetails = new List<object>();
            foreach (var user in deliveryRiderRole)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDetails = new
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles
                };
                deliveryRiderDetails.Add(userDetails);
            }

            return Ok(deliveryRiderDetails);
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

        // GET: api/DeliveryRider/GetDeliveryRiderDetails
        [HttpGet("GetDeliveryRiderDetails")]
        public async Task<IActionResult> GetDeliveryRiderDetails(string riderName)
        {
            var deliveryRiders = await _context.DiliveryRIders.Where(r => r.RiderName == riderName).ToListAsync();

            if (deliveryRiders == null || !deliveryRiders.Any())
            {
                return NotFound("No delivery rider details found for the specified rider name.");
            }

            return Ok(deliveryRiders);
        }

        // POST: api/DeliveryRider/SendMessage
        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage(string userId, List<int> orderIds)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (orderIds == null || !orderIds.Any())
            {
                return BadRequest("No orders specified.");
            }

            var messageDetails = new List<object>();

            foreach (var orderId in orderIds)
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.DeliveryStatus == "Pending");
                if (order != null)
                {
                    // Check if there is already a pending message for the user and order ID
                    var existingPendingMessage = await _context.PendingMessages.FirstOrDefaultAsync(p => p.UserId == userId && p.Message.Contains($"Order ID: {orderId}"));
                    if (existingPendingMessage != null)
                    {
                        // Update the existing pending message with the new order details
                        existingPendingMessage.Message = $"Order ID: {orderId}, Seller: {order.SellerName}, Item: {order.ItemName}, Quantity: {order.Quantity}, Total Price: {order.TotalPrice}";
                        existingPendingMessage.DateTime = DateTime.Now;

                        _context.PendingMessages.Update(existingPendingMessage);
                        await _context.SaveChangesAsync();

                        var messageWithConfirmation = new
                        {
                            Message = existingPendingMessage.Message,
                            Options = new
                            {
                                Accept = Url.Action("AcceptMessage", new { userId }),
                                Reject = Url.Action("RejectMessage", new { userId })
                            }
                        };

                        messageDetails.Add(messageWithConfirmation);
                    }
                    else
                    {
                        // If there is no existing pending message, create a new one
                        var pendingMessage = new PendingMessage
                        {
                            UserId = userId,
                            Message = $"Order ID: {orderId}, Seller: {order.SellerName}, Item: {order.ItemName}, Quantity: {order.Quantity}, Total Price: {order.TotalPrice}",
                            Action = "requested", // Initial action is requested
                            DateTime = DateTime.Now
                        };

                        _context.PendingMessages.Add(pendingMessage);
                        await _context.SaveChangesAsync();

                        var messageWithConfirmation = new
                        {
                            Message = pendingMessage.Message,
                            Options = new
                            {
                                Accept = Url.Action("AcceptMessage", new { userId }),
                                Reject = Url.Action("RejectMessage", new { userId })
                            }
                        };

                        messageDetails.Add(messageWithConfirmation);
                    }
                }
                else
                {
                    return NotFound($"Order with ID {orderId} not found or not pending.");
                }
            }

            return Ok(messageDetails);
        }


        // POST: api/DeliveryRider/AcceptMessage
        [HttpPost("AcceptMessage")]
        public async Task<IActionResult> AcceptMessage(string userId)
        {
            var pendingMessage = await _context.PendingMessages.FindAsync(userId);
            if (pendingMessage == null)
            {
                return BadRequest("No pending message found for the user.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var orderIds = GetOrderIdsFromMessage(pendingMessage.Message);

            if (orderIds == null || !orderIds.Any())
            {
                return BadRequest("No order IDs found in the message.");
            }

            foreach (var orderId in orderIds)
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.DeliveryStatus == "Pending");
                if (order == null)
                {
                    return NotFound($"Order with ID {orderId} not found or not pending.");
                }

                order.DeliveryStatus = "On Way"; // Update the delivery status of the order to "On Way"

                var deliveryRider = new DiliveryRIder
                {
                    SellerName = order.SellerName,
                    CustomerName = order.UserName,
                    RiderName = user.UserName,
                    OrderId = order.Id, // Save the order ID
                    OrderDetails = pendingMessage.Message,
                    DiliveryStatus = "In Progress"
                };

                _context.DiliveryRIders.Add(deliveryRider);
            }

            pendingMessage.Action = "accepted"; // Update action to accepted

            _context.PendingMessages.Remove(pendingMessage); // Remove the pending message
            await _context.SaveChangesAsync();

            return Ok("Message accepted successfully, order statuses updated, and orders saved to the database.");
        }

        // Helper method to extract order IDs from the message
        private List<int> GetOrderIdsFromMessage(string message)
        {
            var orderIds = new List<int>();
            var substrings = message.Split(", ", StringSplitOptions.RemoveEmptyEntries);

            foreach (var substring in substrings)
            {
                var parts = substring.Split(": ");
                if (parts.Length == 2 && parts[0].Trim() == "Order ID")
                {
                    if (int.TryParse(parts[1].Trim(), out int orderId))
                    {
                        orderIds.Add(orderId);
                    }
                }
            }

            return orderIds;
        }

        // POST: api/DeliveryRider/RejectMessage
        [HttpPost("RejectMessage")]
        public async Task<IActionResult> RejectMessage(string userId)
        {
            var pendingMessage = await _context.PendingMessages.FindAsync(userId);
            if (pendingMessage == null)
            {
                return BadRequest("No pending message found for the user.");
            }

            pendingMessage.Action = "rejected"; // Update action to rejected
            _context.PendingMessages.Remove(pendingMessage); // Remove the pending message
            await _context.SaveChangesAsync();

            return Ok("Message rejected successfully.");
        }

        // POST: api/DeliveryRider/MarkDelivered
        [HttpPost("MarkDelivered")]
        public async Task<IActionResult> MarkDelivered(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            order.DeliveryStatus = "Delivered Successfully"; // Update the delivery status of the order to "Delivered Successfully"

            // Find the corresponding delivery rider record and update its status
            var deliveryRider = await _context.DiliveryRIders
                .FirstOrDefaultAsync(d => d.OrderId == orderId && d.DiliveryStatus == "In Progress");

            if (deliveryRider != null)
            {
                deliveryRider.DiliveryStatus = "Delivered Successfully";
                _context.Entry(deliveryRider).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return Ok("Delivery status updated to Delivered Successfully.");
        }


        // GET: api/DeliveryRider/GetPendingMessages
        [HttpGet("GetPendingMessages")]
        public async Task<IActionResult> GetPendingMessages(string userId)
        {
            var pendingMessages = await _context.PendingMessages
                .Where(p => p.UserId == userId && p.Action == "requested")
                .ToListAsync();

            if (pendingMessages == null || !pendingMessages.Any())
            {
                return NotFound("No pending messages found for the specified user.");
            }

            return Ok(pendingMessages);
        }

        // GET: api/DeliveryRider/GetPendingMessages
        [HttpGet("GetProgress/DiliveryRider")]
        public async Task<IActionResult> GetProgressMessages(string Name)
        {
            var pendingMessages = await _context.DiliveryRIders
                .Where(d => d.RiderName == Name && d.DiliveryStatus == "In Progress")
                .ToListAsync();

            if (pendingMessages == null || !pendingMessages.Any())
            {
                return NotFound("No pending messages found for the specified user.");
            }

            return Ok(pendingMessages);
        }

        // GET: api/DeliveryRider/GetPendingMessages
        [HttpGet("DiliviredSucess/DiliveryRider")]
        public async Task<IActionResult> GetSucess(string Name)
        {
            var pendingMessages = await _context.DiliveryRIders
                .Where(d => d.RiderName == Name && d.DiliveryStatus == "Delivered Successfully")
                .ToListAsync();

            if (pendingMessages == null || !pendingMessages.Any())
            {
                return NotFound("No pending messages found for the specified user.");
            }

            return Ok(pendingMessages);
        }
    }
}
