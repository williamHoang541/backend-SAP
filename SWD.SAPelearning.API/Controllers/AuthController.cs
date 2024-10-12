using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using System.Threading.Tasks;

namespace SWD.SAPelearning.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;

        public AuthController(IAuth authService)
        {
            _authService = authService;
        }

        // Endpoint đăng nhập bằng Google
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] string idToken)
        {
            if (string.IsNullOrEmpty(idToken))
            {
                return BadRequest(new { message = "Google token is required" });
            }

            try
            {
                // Xử lý đăng nhập Google với token và lấy thông tin người dùng
                var userInfo = await _authService.HandleGoogleLoginAsync(idToken);

                // Tạo JWT token sau khi xác thực thành công
                var jwtToken = _authService.GenerateJwtToken(userInfo.Email);

                // Trả về thông tin người dùng và JWT token
                return Ok(new
                {
                    UserInfo = userInfo,
                    Token = jwtToken
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = "Login failed: " + ex.Message });
            }
        }

        // Endpoint lấy thông tin người dùng
        [HttpGet("user-info")]
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {
                var userInfo = await _authService.GetUserInfoFromGoogleAsync();
                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
