using SAPelearning_bakend.DTO.UserDTO;
using SWD.SAPelearning.Repository.DTO.UserDTO;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface IUser
    {
        Task<List<Usertb>> GetAllUsers();
        Task<string> Login(LoginDTO request);
        Task<Usertb> Registration(RegisterDTO request);
        Task<Usertb> CreateInstructor(CreateUserInstructorDTO request);
        Task<Usertb> UpdateStudent(string id, UpdateUserStudentDTO user);
        Task<List<Usertb>> SearchByName(string name);
        Task<Usertb> getUserByID(SearchUserIdDTO id);
        Task<Usertb> UpdateStatusIsOnline(string userID);
        Task<List<Usertb>> GetStudentsByPrefix(string userIdPrefix);
        Task<bool> Delete(RemoveUDTO id);
    }
}
