using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.Models;
using SWD.SAPelearning.Repository.DTO.PaymentDTO;
using SWD.SAPelearning.Service;


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
        [HttpPost]
        public IActionResult CreatePayment([FromBody] PaymentDTO paymentDto)
        {
            if (paymentDto == null)
            {
                return BadRequest("Payment data is null.");
            }

            var createdPayment = this.payment.CreatePayment(paymentDto);
            return CreatedAtAction(nameof(GetPaymentById), new { id = createdPayment.Id }, createdPayment);
        }

        // GET: api/payment/{id}
        [HttpGet("{id}")]
        public IActionResult GetPaymentById(int id)
        {
            var payment = this.payment.GetPaymentById(id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        // GET: api/payment/enrollment/{enrollmentId}
        [HttpGet("enrollment/{enrollmentId}")]
        public IActionResult GetPaymentsByEnrollmentId(int enrollmentId)
        {
            var payments = this.payment.GetPaymentsByEnrollmentId(enrollmentId);
            return Ok(payments);
        }


    }
}
