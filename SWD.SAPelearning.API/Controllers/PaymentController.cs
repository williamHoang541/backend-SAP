using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;


namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPayment paymentS;

        public PaymentController(IPayment payment)
        {
            this.paymentS = payment;
        }

        // POST: api/payment/create
        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] string enrollmentId)
        {
            try
            {
                var payment = await paymentS.createPayment(enrollmentId);
                if (payment == null)
                {
                    return BadRequest("Failed to create payment. Enrollment not found.");
                }
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/payment/delete/{paymentId}
        [HttpDelete("delete/{paymentId}")]
        public async Task<IActionResult> DeletePayment(string paymentId)
        {
            try
            {
                var payment = await paymentS.DeletePayment(paymentId);
                if (payment == null)
                {
                    return NotFound("Payment not found.");
                }
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/payment/delete-complete/{paymentId}
        [HttpDelete("delete-complete/{paymentId}")]
        public async Task<IActionResult> DeletePaymentComplete(string paymentId)
        {
            try
            {
                var result = await paymentS.DeletePaymentComplete(paymentId);
                if (!result)
                {
                    return NotFound("Payment not found or could not be deleted.");
                }
                return Ok("Payment successfully deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/payment/{enrollmentId}
        [HttpGet("{enrollmentId}")]
        public async Task<IActionResult> GetPayment(string enrollmentId)
        {
            try
            {
                var payment = await paymentS.GetPayment(enrollmentId);
                if (payment == null)
                {
                    return NotFound("Payment not found.");
                }
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/payment/fail/{enrollmentId}
        [HttpGet("fail/{enrollmentId}")]
        public async Task<IActionResult> GetPaymentFail(string enrollmentId)
        {
            try
            {
                var payment = await paymentS.GetPaymentFail(enrollmentId);
                if (payment == null)
                {
                    return NotFound("Failed payment not found.");
                }
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/payment/success/{enrollmentId}
        [HttpGet("success/{enrollmentId}")]
        public async Task<IActionResult> GetPaymentSuccess(string enrollmentId)
        {
            try
            {
                var payment = await paymentS.GetPaymentSuccess(enrollmentId);
                if (payment == null)
                {
                    return NotFound("Successful payment not found.");
                }
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: api/payment/update/{paymentId}
        [HttpPut("update/{paymentId}")]
        public async Task<IActionResult> UpdatePayment(string paymentId)
        {
            try
            {
                var payment = await paymentS.UpdatePayment(paymentId);
                if (payment == null)
                {
                    return NotFound("Payment not found or could not be updated.");
                }
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

