using SAPelearning_bakend.DTO.UserDTO;
using SWD.SAPelearning.Repository.DTO.UserDTO;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface IUser
    {
        Task<List<User>> GetAllUsers();
        Task<string> LoginApp(LoginDTO request);
        Task<string> LoginWeb(LoginDTO request);
        Task<User> Registration(RegisterDTO request);
        Task<User> CreateInstructor(CreateUserInstructorDTO request);
        Task<User> UpdateStudent(string id, UpdateUserStudentDTO user);
        Task<List<User>> SearchByName(string name);
        Task<User> getUserByID(SearchUserIdDTO id);
        Task<User> UpdateStatusIsOnline(string userID);
        Task<List<User>> GetStudentsByPrefix(string userIdPrefix);
        Task<bool> Delete(RemoveUDTO id);
    }
}
