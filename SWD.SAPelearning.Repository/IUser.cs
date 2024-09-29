using SWD.SAPelearning.Repository.DTO.UserDTO;
using SWD.SAPelearning.Repository.Models;

namespace SWD.SAPelearning.Repository
{
    public interface IUser
    {
        Task<List<Usertb>> GetAllUsers();
        Task<string> Login(LoginDTO request);
        Task<Usertb> Registration(RegisterDTO request);
        Task<bool> Delete(RemoveDTO id);
    }
}
