using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.Models;
using SWD.SAPelearning.Repository.DTO.UserDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SAPelearning_bakend.DTO.UserDTO;

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
                // Retrieve the user based on the username
                var user = await this.context.Usertbs.Where(x => x.Username.Equals(request.Username))
                                                     .Include(y => y.Roles)
                                                     .FirstOrDefaultAsync();

                if (user == null)
                    throw new Exception("USER IS NOT FOUND");

                // Check if the password is correct
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                    throw new Exception("INVALID PASSWORD");

                user.IsOnline = true;
                this.context.Usertbs.Update(user);
                await this.context.SaveChangesAsync();
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

        public async Task<Usertb> CreateInstructor(CreateUserInstructorDTO request)
        {
            try
            {
                var instructor = new Usertb();
                if (request != null)
                {
                    // Validate Gender
                    var validGenders = new[] { "Male", "Female" };
                    if (!validGenders.Contains(request.Gender, StringComparer.OrdinalIgnoreCase))
                    {
                        throw new Exception("Gender must be either 'Male' or 'Female'.");
                    }

                    // Check if username or email already exists
                    foreach (var x in this.context.Usertbs)
                    {
                        if (request.Username.Equals(x.Username))
                        {
                            throw new Exception("Username already exists!");
                        }
                        if (request.Email.Equals(x.Email))
                        {
                            throw new Exception("Email already exists!");
                        }
                    }

                    // Create unique UserId for Instructor
                    instructor.Userid = "I" + Guid.NewGuid().ToString().Substring(0, 5);
                    instructor.Username = request.Username;
                    instructor.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                    instructor.Email = request.Email;
                    instructor.Fullname = request.Fullname;
                    instructor.Education = request.Education;
                    instructor.Phonenumber = request.PhoneNumber;
                    instructor.Gender = request.Gender;
                    instructor.Roleid = "2"; 
                    instructor.LastLogin = DateTime.Now;
                    instructor.IsOnline = false;

                    // Add the instructor to the database
                    await this.context.Usertbs.AddAsync(instructor);
                    await this.context.SaveChangesAsync();
                    return instructor;
                }
                return null;
            }
            catch (Exception ex)
            {
                var errorMessage = $"{ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}";
                throw new Exception(errorMessage);
            }
        }

        public async Task<Usertb> UpdateStudent(string id, UpdateUserStudentDTO user)
        {
            try
            {
                var existingUser = await context.Usertbs.FirstOrDefaultAsync(x => x.Userid == id);
                if (existingUser == null)
                    throw new Exception("USER NOT FOUND");

                // Validate email format if provided
                if (!string.IsNullOrEmpty(user.Email))
                {
                    var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; // Basic email validation regex
                    if (!System.Text.RegularExpressions.Regex.IsMatch(user.Email, emailPattern))
                    {
                        throw new Exception("Invalid email format.");
                    }

                    // Check if email already exists in the system (excluding the current user)
                    var emailExists = await context.Usertbs.AnyAsync(x => x.Email == user.Email && x.Userid != id);
                    if (emailExists)
                    {
                        throw new Exception("Email already exists.");
                    }
                }

                // Validate Gender
                var validGenders = new[] { "Male", "Female" };
                if (!string.IsNullOrEmpty(user.Gender) && !validGenders.Contains(user.Gender, StringComparer.OrdinalIgnoreCase))
                {
                    throw new Exception("Gender must be either 'Male' or 'Female'.");
                }

                // Update properties if the value is not null
                existingUser.Email = user.Email ?? existingUser.Email;
                existingUser.Fullname = user.Fullname ?? existingUser.Fullname;
                existingUser.Education = user.Education ?? existingUser.Education;
                existingUser.Phonenumber = user.PhoneNumber ?? existingUser.Phonenumber;
                existingUser.Gender = user.Gender ?? existingUser.Gender;

                // Update user in the database
                context.Usertbs.Update(existingUser);
                await context.SaveChangesAsync();

                return existingUser;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<List<Usertb>> SearchByName(string name)
        {
            try
            {
                var list = await this.context.Usertbs.Where(x => x.Username.Contains(name)).ToListAsync();
                if (list != null) return list;
                throw new Exception("Not Found");
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<Usertb> getUserByID(SearchUserIdDTO id)
        {
            try
            {
                var search = await this.context.Usertbs.Where(x => x.Userid.Equals(id.userID))
                                                                .FirstOrDefaultAsync();
                return search;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<Usertb> UpdateStatusIsOnline(string userID)
        {
            try
            {
                // Find the user by userID
                var user = await this.context.Usertbs.Where(a => a.Userid == userID).FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                // Toggle the IsOnline status
                user.IsOnline = !user.IsOnline;

                // Update the user's record
                this.context.Usertbs.Update(user);
                await this.context.SaveChangesAsync();

                return user;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<bool> Delete(RemoveUDTO id)
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
