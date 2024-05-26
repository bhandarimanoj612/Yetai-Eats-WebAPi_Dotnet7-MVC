using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Yetai_Eats.Model.DTO;

namespace Yetai_Eats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Payment : ControllerBase
    {
        [HttpPost("intents")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentIntentRequest request)
        {
            try
            {
                // Set your Stripe API key
                StripeConfiguration.ApiKey = "sk_test_51OxTFa1bF7cmrKQ8T0NIKQIcJVCUoGa2jA1VRJv45oBp86wRgsEHDPvTcjKM2WmVxJLuOklVn4VxWI0sDPHmaMpq007GZmiWMv";

                // Create a PaymentIntent
                var paymentIntentOptions = new PaymentIntentCreateOptions
                {
                    Amount = request.Amount,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" },
                    SetupFutureUsage = "off_session"
                };
                var paymentIntentService = new PaymentIntentService();
                var paymentIntent = await paymentIntentService.CreateAsync(paymentIntentOptions);

                // Return the client secret
                return Ok(new { clientSecret = paymentIntent.ClientSecret });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
