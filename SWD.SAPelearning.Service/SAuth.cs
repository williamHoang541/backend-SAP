using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO.UserDTO;
using SWD.SAPelearning.Repository.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SWD.SAPelearning.Service
{
    public class SAuth : IAuth
    {
        private readonly IUser _userRepository; // Repository để quản lý người dùng
        private readonly IConfiguration _configuration;   // Để lấy thông tin cấu hình JWT

        public SAuth(IUser userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        // Kiểm tra xem tài khoản có tồn tại hay không qua email
        public async Task<bool> CheckAccountByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            return user != null;
        }

        // Tạo JWT token trực tiếp trong phương thức này
        public async Task<string> CreateTokenByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            // Tạo token bằng cách sử dụng JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]); // Khóa bí mật từ file cấu hình

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Fullname)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token sẽ hết hạn sau 1 giờ
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token); // Trả về token đã mã hóa
        }

        // Tạo tài khoản người dùng mới nếu chưa có tài khoản
        public async Task<User> CreateNewUserAccountByGoogle(string email, string name)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                return null; // Nếu đã có tài khoản, trả về null
            }

            // Tạo tài khoản người dùng mới
            var newUser = new User
            {
                Email = email,
                Fullname = name,
                RegistrationDate = DateTime.UtcNow
            };

            await _userRepository.CreateUserAsync(newUser); // Lưu vào cơ sở dữ liệu
            return newUser;
        }
    }
}