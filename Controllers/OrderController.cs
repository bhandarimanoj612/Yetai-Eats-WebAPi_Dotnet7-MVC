using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yetai_Eats.Data;
using Yetai_Eats.Model;
using Yetai_Eats.Model.DTO;

namespace Yetai_Eats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        //for creating order

        // GET: api/Admin/TotalStats
        [HttpGet("Admin/TotalStats/")]
        public IActionResult GetTotalStats()
        {
            try
            {
                // Calculate total number of orders
                int totalOrders = _context.Orders.Count();

                // Calculate total income from all menu items
                double totalMenuIncome = _context.Orders.Sum(o => o.TotalPrice);

                // Calculate total income from all individual items
                double totalIndividualItemIncome = _context.Orders.Where(o => o.Category == "IndividualMenuItem").Sum(o => o.TotalPrice);

                // Calculate total income from all menu items
                double totalMenuItemIncome = _context.Orders.Where(o => o.Category == "MenuItems").Sum(o => o.TotalPrice);

                // Prepare response object
                var stats = new
                {
                    TotalOrders = totalOrders,
                    TotalMenuIncome = totalMenuIncome,
                    TotalIndividualItemIncome = totalIndividualItemIncome,
                    TotalMenuItemIncome = totalMenuItemIncome
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        // GET: api/Order/UserStats/{businessName}
        [HttpGet("UserStats/{businessName}")]
        public IActionResult GetUserStatsByBusinessName(string businessName)
        {
            try
            {
                // Get orders for the specified business name
                var orders = _context.Orders.Where(o => o.SellerName == businessName).ToList();

                // Calculate total number of orders
                int totalOrders = orders.Count;

                // Calculate total delivery count
                int totalDeliveries = orders.Count(o => o.DeliveryStatus == "Delivered");

                // Calculate total sale
                double totalSale = orders.Sum(o => o.TotalPrice);

                // Prepare response object
                var stats = new
                {
                    BusinessName = businessName,
                    TotalOrders = totalOrders,
                    TotalDeliveries = totalDeliveries,
                    TotalSale = totalSale
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult CreateOrder(OrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = new Order
            {
                SellerName = orderDTO.SellerName,
                UserName = orderDTO.UserName,
                ItemName = orderDTO.ItemName,
                OrderTime = DateTime.Now,
                Quantity = orderDTO.Quantity,
                Stripe = orderDTO.Stripe,
                TotalPrice = orderDTO.TotalPrice,
                Email = orderDTO.Email,
                Category= orderDTO.Category,
                Address = orderDTO.Address,
                PhoneNumber = orderDTO.PhoneNumber,
                DeliveryStatus= "Pending"

            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            // Return the Id of the newly created order
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }
        [HttpPost("createOrderS")]
        public IActionResult CreateOrderS(OrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sellerNames = orderDTO.SellerName.Split(", ");
            var itemNames = orderDTO.ItemName.Split(", ");
            var emails = orderDTO.Email.Split(", ");
            var categories = orderDTO.Category.Split(",");

            // Ensure all arrays have the same length
            var maxLength = Math.Max(sellerNames.Length, Math.Max(itemNames.Length, Math.Max(emails.Length, categories.Length)));

            var orderIds = new List<int>(); // Store the IDs of the created orders

            for (int i = 0; i < maxLength; i++)
            {
                var sellerName = i < sellerNames.Length ? sellerNames[i] : sellerNames.LastOrDefault();
                var itemName = i < itemNames.Length ? itemNames[i] : itemNames.LastOrDefault();
                var email = i < emails.Length ? emails[i] : emails.LastOrDefault();
                var category = i < categories.Length ? categories[i] : categories.LastOrDefault();

                var order = new Order
                {
                    SellerName = sellerName,
                    UserName = orderDTO.UserName,
                    ItemName = itemName,
                    OrderTime = DateTime.Now,
                    Quantity = orderDTO.Quantity,
                    Stripe = orderDTO.Stripe,
                    TotalPrice = orderDTO.TotalPrice,
                    Email = email,
                    Category = category,
                    DeliveryStatus = "Pending", // Assuming the delivery status is updated here
                    Address = orderDTO.Address,
                    PhoneNumber = orderDTO.PhoneNumber,
                   
                };

                _context.Orders.Add(order);
                _context.SaveChanges();

                orderIds.Add(order.Id);
            }

            // Return the IDs of the newly created orders
            return Ok(orderIds);
        }

       


        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet("userList/{username}")]
        public IActionResult GetOrdersByUsernameFullList(string username)
        {
            var orders = _context.Orders.Where(o => o.UserName == username).ToList();
            if (orders == null || orders.Count == 0)
            {
                return NotFound("No orders found for the given username");
            }
            return Ok(orders);
        }
        [HttpGet("emailList/{email}")]
        public IActionResult GetOrdersByEmailList(string email)
        {
            var orders = _context.Orders.Where(o => o.Email == email).ToList();
            if (orders == null || orders.Count == 0)
            {
                return NotFound("No orders found for the given email");
            }
            return Ok(orders);
        }

       
        [HttpPost("UpdateRateOrderItem")]
        public IActionResult UpdateRateOrderItem(RatingDTO ratingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the order item based on seller name, item name, and email
            var order = _context.MenuItems.FirstOrDefault(o =>
                o.BusinessName == ratingDTO.SellerName &&
                o.ItemName == ratingDTO.ItemName &&
                o.Email == ratingDTO.Email);
               

            // Save changes to the database
            _context.SaveChanges();

            return Ok("Rating saved successfully");
        }


        [HttpGet("userS/{username}")]
        public IActionResult GetOrdersByUsername(string username)
        {
            var orders = _context.Orders.Where(o => o.UserName == username).ToList();
            if (orders == null || orders.Count == 0)
            {
                return NotFound("No orders found for the given username");
            }

            var result = new List<Order>();

            foreach (var order in orders)
            {
                var sellerNames = order.SellerName.Split(", ");
                var itemNames = order.ItemName.Split(", ");
                var emails = order.Email.Split(", ");
                var categories = order.Category.Split(",");

                // Ensure all arrays have the same length
                var maxLength = Math.Max(sellerNames.Length, Math.Max(itemNames.Length, Math.Max(emails.Length, categories.Length)));

                for (int i = 0; i < maxLength; i++)
                {
                    var sellerName = i < sellerNames.Length ? sellerNames[i] : sellerNames.LastOrDefault();
                    var itemName = i < itemNames.Length ? itemNames[i] : itemNames.LastOrDefault();
                    var email = i < emails.Length ? emails[i] : emails.LastOrDefault();
                    var category = i < categories.Length ? categories[i] : categories.LastOrDefault(); 
                    // Set category to null if there are no more categories

                    var newOrder = new Order
                    {
                        Id = order.Id,
                        SellerName = sellerName,
                        UserName = order.UserName,
                        ItemName = itemName,
                        OrderTime = order.OrderTime,
                        Quantity = order.Quantity,
                        Stripe = order.Stripe,
                        TotalPrice = order.TotalPrice,
                        Email = email,
                        Category = category,
                    };
                    result.Add(newOrder);
                }
            }
            return Ok(result);
        }





        [HttpGet("emailS/{email}")]
        public IActionResult GetOrdersByEmailS(string email)
        {
            var orders = _context.Orders.Where(o => o.Email == email).ToList();
            if (orders == null || orders.Count == 0)
            {
                return NotFound("No orders found for the given email");
            }

            var result = new List<Order>();

            foreach (var order in orders)
            {
                var sellerNames = order.SellerName.Split(", ");
                var itemNames = order.ItemName.Split(", ");
                var emails = order.Email.Split(", ");
                var categories = order.Category.Split(",");

                // Ensure all arrays have the same length
                var maxLength = Math.Max(sellerNames.Length, Math.Max(itemNames.Length, Math.Max(emails.Length, categories.Length)));

                for (int i = 0; i < maxLength; i++)
                {
                    var sellerName = i < sellerNames.Length ? sellerNames[i] : sellerNames.LastOrDefault();
                    var itemName = i < itemNames.Length ? itemNames[i] : itemNames.LastOrDefault();
                    var emailValue = i < emails.Length ? emails[i] : emails.LastOrDefault();
                    var category = i < categories.Length ? categories[i] : categories.LastOrDefault();

                    // Split the email further
                    var individualEmails = emailValue.Split(",");

                    // Iterate over individual emails
                    foreach (var individualEmail in individualEmails)
                    {
                        var newOrder = new Order
                        {
                            Id = order.Id,
                            SellerName = sellerName,
                            UserName = order.UserName,
                            ItemName = itemName,
                            OrderTime = order.OrderTime,
                            Quantity = order.Quantity,
                            Stripe = order.Stripe,
                            TotalPrice = order.TotalPrice,
                            Email = individualEmail.Trim(), // Trim to remove any leading or trailing whitespace
                            Category = category,
                        };
                        result.Add(newOrder);
                    }
                }
            }
            return Ok(result);
        }



    }
}
