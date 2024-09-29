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
        [Route("get-all-payment")]
        public async Task<IActionResult> GetAll()
        {

            var a = await this.payment.GetAllPayment();
            if (a == null)
            {
                return NotFound();
            }
            return Ok(a);
        }
    }
}
