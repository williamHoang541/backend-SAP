using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICourse
    {
        Task<List<Course>> GetAllCourse();
        Task<Course> CreateCourse(CourseDTO request);
        Task<Course> GetCourseById(int id);

    }
}
