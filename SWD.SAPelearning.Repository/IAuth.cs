using SWD.SAPelearning.Repository.DTO.UserDTO;
using SWD.SAPelearning.Repository.Models;
using System.Threading.Tasks;

namespace SWD.SAPelearning.Repository
{
    public interface IAuth
    {
        Task<bool> CheckAccountByEmail(string email);
        Task<string> CreateTokenByEmail(string email);
        Task<User> CreateNewUserAccountByGoogle(string email, string name);
    }
}