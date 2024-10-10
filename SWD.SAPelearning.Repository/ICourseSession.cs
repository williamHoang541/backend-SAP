using SWD.SAPelearning.Repository.DTO.CourseSessionDTO;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface ICourseSession
    {
        Task<List<CourseSession>> GetAllCourseSession();
        Task<CourseSessionDTO> CreateCourseSession(CourseSessionDTO request); // Create a new course session
        Task<CourseSessionDTO> UpdateCourseSession(int id, CourseSessionDTO request); // Update an existing course session by ID
        Task<bool> DeleteCourseSession(int id); // Delete a course session by ID
        Task<CourseSessionDTO> GetCourseSessionById(int id);
    }
}
