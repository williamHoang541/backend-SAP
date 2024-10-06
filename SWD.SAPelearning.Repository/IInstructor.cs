using SWD.SAPelearning.Repository.DTO.Instructor;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface IInstructor
    {
        Task<List<Instructor>> GetAllInstructor();
        Task<bool> UpdateInstructorByUserId(string userId, UpdateInstructorDTO updateInstructorDTO);
        Task<bool> RemoveInstructor(RemoveIDTO removeIDTO);
    }
}
