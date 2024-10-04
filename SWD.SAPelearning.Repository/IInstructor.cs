using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface IInstructor
    {
        Task<List<Instructor>> GetAllInstructor();
    }
}
