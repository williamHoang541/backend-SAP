using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICourse
    {
        Task<List<Course>> GetAllCourse();
    }
}
