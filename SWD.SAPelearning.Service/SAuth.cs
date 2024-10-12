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
        private readonly IConfiguration _configuration;

        public SAuth(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Xử lý đăng nhập với Google và trả về thông tin người dùng
        public async Task<UserInfoDTO> HandleGoogleLoginAsync(string idToken)
        {
            try
            {
                // Xác thực token từ Google và lấy thông tin người dùng
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

                // Tạo đối tượng UserInfoDTO để trả về thông tin người dùng
                var userInfo = new UserInfoDTO
                {
                    Email = payload.Email,
                    Fullname = payload.Name,
                    
                };

                return userInfo;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid Google Token: " + ex.Message);
            }
        }

        // Lấy thông tin người dùng từ Google (nếu cần)
        public async Task<User> GetUserInfoFromGoogleAsync()
        {
            // Ví dụ cách lấy thông tin người dùng từ Google bằng token ID
            var payload = await GoogleJsonWebSignature.ValidateAsync("google-id-token");

            if (payload == null)
            {
                throw new Exception("Invalid Google token");
            }

            var userInfo = new User
            {
                Fullname = payload.Name,
                Email = payload.Email,
                Phonenumber = payload.Email // Có thể thay thế bằng số điện thoại nếu có
            };

            return userInfo;
        }

        // Tạo JWT Token
        public string GenerateJwtToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}