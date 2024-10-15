using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Service;

namespace SWD.SAPelearning.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentPaypalController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentPaypalController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create")]
        public IActionResult CreatePayment(decimal amount)
        {
            string returnUrl = "http://localhost:5250/success";
            string cancelUrl = "http://localhost:5250/cancel";

            var payment = _paymentService.CreatePayment(returnUrl, cancelUrl, amount);

            var approvalUrl = payment.links.FirstOrDefault(link => link.rel == "approval_url")?.href;

            return Ok(new { approvalUrl });
        }

        [HttpGet("success")]
        public IActionResult Success(string paymentId, string payerId)
        {
            var payment = _paymentService.ExecutePayment(paymentId, payerId);
            if (payment.state.ToLower() == "approved")
            {
                return Ok("Payment successful!");
            }
            return BadRequest("Payment failed.");
        }

        [HttpGet("cancel")]
        public IActionResult Cancel()
        {
            return BadRequest("Payment was canceled.");
        }
    }
}

