using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.Models;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SPayment : IPayment
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningAPIContext context;

        public SPayment(SAPelearningAPIContext Context, IConfiguration configuration)
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

        public async Task<Payment> CreatePayment(string enrollmentId)
        {
            // Find the enrollment related to the given enrollmentId
            var enrollment = await context.Enrollments.FindAsync(Convert.ToInt32(enrollmentId));

            if (enrollment == null)
            {
                throw new Exception("Enrollment not found.");
            }

            // Create a new payment
            var payment = new Payment
            {
                EnrollmentId = enrollment.Id,
                Amount = enrollment.Course.Price, // Assuming the amount is based on course price
                PaymentDate = DateTime.Now,
                TransactionId = new Random().Next(10000, 99999), // You may have a method to generate this
                Status = "Pending" // Or another default status
            };

            // Add payment to the context
            context.Payments.Add(payment);

            // Save changes to the database
            await context.SaveChangesAsync();

            return payment;
        }

        public async Task<Payment> GetPaymentById(int id)
        {
            // Use FindAsync to retrieve the payment by its ID
            var payment = await context.Payments.FindAsync(id);

            // Return the payment if found, otherwise null
            return payment;
        }

        public async Task<List<Payment>> GetPaymentsByEnrollmentId(int enrollmentId)
        {
            // Use Where to filter payments by EnrollmentId
            var payments = await context.Payments
                .Where(p => p.EnrollmentId == enrollmentId)
                .ToListAsync(); // Retrieve the list of payments

            return payments; // Return the list of payments found
        }
    }
}
