using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface IPayment
    {
        Task<List<Payment>> GetAllPayment();
    }
}
