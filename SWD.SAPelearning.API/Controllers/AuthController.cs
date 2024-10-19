using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using SWD.SAPelearning.Repository;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SWD.SAPelearning.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _auth;

        public AuthController(IAuth auth)
        {
            _auth = auth;
        }

        // Google Sign-In
        [HttpGet("google-auth/signin")]
        public IActionResult GoogleSignIn()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action("api/auth/google-auth/signin")
            };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        // Callback after Google Sign-In
        [HttpGet("google-auth/signin-callback")]
        public async Task<IActionResult> GoogleSignInCallback()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded || result.Principal == null)
            {
                return Unauthorized(new
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Code = "FAILURE",
                    Message = "Google authentication failed"
                });
            }

            var claims = result.Principal.Claims;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email not found.");
            }

            // Check if user exists by email
            var userExists = await _auth.CheckAccountByEmail(email);
            if (!userExists)
            {
                return Unauthorized(new
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Code = "FAILURE",
                    Message = "Account not found. Please sign up first."
                });
            }

            // Generate token for existing user
            var token = await _auth.CreateTokenByEmail(email);

            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Code = "SUCCESS",
                Data = new
                {
                    Email = email,
                    Token = token
                }
            });
        }

        // Google Sign-Up
        [HttpGet("google-auth/signup")]
        public IActionResult GoogleSignUp()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action("api/auth/google-auth/signup")
            };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        // Callback after Google Sign-Up
        [HttpGet("google-auth/signup-callback")]
        public async Task<IActionResult> GoogleSignUpCallback()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded || result.Principal == null)
            {
                return Unauthorized(new
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Code = "FAILURE",
                    Message = "Google authentication failed"
                });
            }

            var claims = result.Principal.Claims;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email not found.");
            }

            // Check if user already exists
            var userExists = await _auth.CheckAccountByEmail(email);
            if (userExists)
            {
                return BadRequest("User already exists. Please sign in.");
            }

            // Create new account
            var createUser = await _auth.CreateNewUserAccountByGoogle(email, name);
            if (createUser == null)
            {
                return Problem("Failed to create account.");
            }

            // Generate token for the new user
            var token = await _auth.CreateTokenByEmail(email);

            return Ok(new
            {
                StatusCode = StatusCodes.Status200OK,
                Code = "SUCCESS",
                Data = new
                {
                    Email = email,
                    Token = token
                }
            });
        }
    }
}
