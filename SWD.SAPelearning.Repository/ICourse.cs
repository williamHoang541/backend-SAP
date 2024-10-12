using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICourse
    {
        Task<List<Course>> GetAllCourse();
        Task<CourseDTO> CreateCourse(CourseDTO request);
        Task<CourseDTO> UpdateCourse(int id, CourseDTO request);
        Task<bool> DeleteCourse(int id);
        Task<CourseDTO> GetCourseById(int id);
    }
}
