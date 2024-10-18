using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface IPayment
    {
        Task<List<Payment>> GetAllPayment();
        Task<Payment> CreatePayment(string enrollmentId);
        Task<Payment> GetPaymentById(int id);
        Task<List<Payment>> GetPaymentsByEnrollmentId(int enrollmentId);
    }
}
