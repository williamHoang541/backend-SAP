using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.Models;
using SWD.SAPelearning.Repository.DTO.UserDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SUser : IUser
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningContext context;

        public SUser(SAPelearningContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }


        public async Task<List<Usertb>> GetAllUsers()
        {
            try
            {
                var a = await this.context.Usertbs.ToListAsync();
                return a;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }
        }

        private string CreateToken(Usertb user)
        {

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, user.Roleid),
                new Claim("userid", user.Userid),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;

        }
        public async Task<string> Login(LoginDTO request)
        {
            try
            {
                var user = await this.context.Usertbs.Where(x => x.Username.Equals(request.Username))
                                                   .Include(y => y.Roles)
                                                   .FirstOrDefaultAsync();
                if (user == null)
                    throw new Exception("USER IS NOT FOUND");
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                    throw new Exception("INVALID PASSWORD");
                //if (!user.Status)
                //    throw new Exception("ACCOUNT WAS BANNED OR DELETED");
                var token = CreateToken(user);
                return token;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<Usertb> Registration(RegisterDTO request)
        {
            try
            {
                var r = new Usertb();
                if (request != null)
                {
                    foreach (var x in this.context.Usertbs)
                    {
                        if (request.Username.Equals(x.Username))
                        {
                            throw new Exception("UserName has been existted!");
                        }
                    }
                    r.Userid = "S" + Guid.NewGuid().ToString().Substring(0, 5);
                    r.Username = request.Username;
                    r.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                    r.Roleid = "3";
                    r.LastLogin= DateTime.Now;
                    r.IsOnline = true;
                    await this.context.Usertbs.AddAsync(r);
                    await this.context.SaveChangesAsync();
                    return r;
                }
                return null;
            }
            catch (Exception ex)
            {
                var errorMessage = $"{ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}";
                throw new Exception(errorMessage);
            }
        }

        public async Task<bool> Delete(RemoveDTO id)
        {
            try
            {
                if (id != null)
                {
                    var obj = await this.context.Usertbs.Where(x => x.Userid.Equals(id.UserID)).FirstOrDefaultAsync();
                    this.context.Usertbs.Remove(obj);
                    await this.context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }
        }
    }
}
