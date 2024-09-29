using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICourseSession
    {
        Task<List<CourseSession>> GetAllCourseSession();
    }
}
