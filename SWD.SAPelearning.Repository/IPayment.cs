using Microsoft.AspNetCore.Http;
using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.PaymentDTO;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface IPayment
    {
        Task<List<Payment>> GetAllPayment();

        PaymentDTO CreatePayment(PaymentDTO paymentDto);
        PaymentDTO GetPaymentById(int id);
        IEnumerable<PaymentDTO> GetPaymentsByEnrollmentId(int enrollmentId);


    }
}
