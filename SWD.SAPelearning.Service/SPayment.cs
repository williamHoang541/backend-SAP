using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.PaymentDTO;
using SWD.SAPelearning.Repository.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SPayment : IPayment
    {
        private readonly IConfiguration _configuration;
        private readonly SAPelearningdeployContext context;
      
        public SPayment(SAPelearningdeployContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
            
        }


        public async Task<List<Payment>> GetAllPayment()
        {
            try
            {
                var a = await this.context.Payments.ToListAsync();
                return a;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }
        }


        public PaymentDTO CreatePayment(PaymentDTO paymentDto)
        {
            var payment = new Payment
            {
                EnrollmentId = paymentDto.EnrollmentId,
                Amount = paymentDto.Amount,
                PaymentDate = paymentDto.PaymentDate,
                TransactionId = paymentDto.TransactionId,
                Status = paymentDto.Status
            };

            context.Payments.Add(payment);
            context.SaveChanges();

            paymentDto.Id = payment.Id;  // Assign the generated ID back to the DTO
            return paymentDto;
        }

        public PaymentDTO GetPaymentById(int id)
        {
            var payment = context.Payments.Find(id);
            if (payment == null) return null;

            return new PaymentDTO
            {
                Id = payment.Id,
                EnrollmentId = payment.EnrollmentId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                TransactionId = payment.TransactionId,
                Status = payment.Status
            };
        }

        public IEnumerable<PaymentDTO> GetPaymentsByEnrollmentId(int enrollmentId)
        {
            return context.Payments
                .Where(p => p.EnrollmentId == enrollmentId)
                .Select(p => new PaymentDTO
                {
                    Id = p.Id,
                    EnrollmentId = p.EnrollmentId,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    TransactionId = p.TransactionId,
                    Status = p.Status
                }).ToList();
        }




    }
}

