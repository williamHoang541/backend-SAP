using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface IPayment
    {
        Task<Payment> createPayment(string EnrollmentId);
        Task<Payment> DeletePayment(string paymentID);
        Task<bool> DeletePaymentComplete(string paymentID);
        Task<Payment> GetPayment(string EnrollmentId);
        Task<Payment> GetPaymentFail(string EnrollmentId);
        Task<Payment> GetPaymentSuccess(string EnrollmentId);
        Task<Payment> UpdatePayment(string id);
    }
}
