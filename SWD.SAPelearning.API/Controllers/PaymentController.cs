using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;


namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPayment payment;

        public PaymentController(IPayment payment)
        {
            this.payment = payment;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.payment.GetAllPayment();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] string enrollmentId)
        {
            if (string.IsNullOrEmpty(enrollmentId))
            {
                return BadRequest("Enrollment ID is required.");
            }

            try
            {
                var payment = await this.payment.CreatePayment(enrollmentId);
                return CreatedAtAction(nameof(GetPaymentById), new { id = payment.Id }, payment);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating payment: {ex.Message}");
            }
        }

        // GET: api/payment/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await this.payment.GetPaymentById(id);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        // GET: api/payment/enrollment/{enrollmentId}
        [HttpGet("enrollment/{enrollmentId}")]
        public async Task<IActionResult> GetPaymentsByEnrollmentId(int enrollmentId)
        {
            // Ensure _paymentService is being used here, not 'this.payment'
            var payments = await this.payment.GetPaymentsByEnrollmentId(enrollmentId);

            if (payments == null || payments.Count == 0)
            {
                return NotFound("No payments found for this enrollment.");
            }

            return Ok(payments);
        }
    }
}

