using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface IEnrollment
    {
        Task<List<Enrollment>> GetAllEnrollment();
    }
}
