using SWD.SAPelearning.Repository.DTO.UserDTO;
using SWD.SAPelearning.Repository.Models;
using System.Threading.Tasks;

namespace SWD.SAPelearning.Repository
{
    public interface IAuth
    {
        // Xử lý đăng nhập với Google và trả về thông tin người dùng
        Task<UserInfoDTO> HandleGoogleLoginAsync(string idToken);

        // Lấy thông tin người dùng từ Google
        Task<User> GetUserInfoFromGoogleAsync();

        // Tạo JWT Token từ email
        string GenerateJwtToken(string email);
    }
}