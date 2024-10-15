using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.DTO.CourseDTO;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICourse
    {
        Task<List<CourseDTO>> GetAllCourseAsync(GetAllDTO getAllDTO);
        Task<CourseDTO> GetCourseById(int id);
        Task<CourseDTO> CreateCourse(CourseCreateDTO request);
        Task<CourseDTO> UpdateCourse(int id, CourseCreateDTO request);
        Task<bool> DeleteCourse(int id);
        Task<int> CountCoursesAsync();
    }
}
