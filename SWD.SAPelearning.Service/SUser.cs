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


        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                var a = await this.context.Users.ToListAsync();
                return a;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }
        }

        private string CreateToken(User user)
        {

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("userid", user.Id),
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

        public async Task<string> LoginApp(LoginDTO request)
        {
            try
            {
                // Truy xuất người dùng dựa trên tên đăng nhập
                var user = await this.context.Users
                    .Where(x => x.Username.Equals(request.Username))
                    .FirstOrDefaultAsync();

                if (user == null)
                    throw new Exception("USER IS NOT FOUND");

                // Kiểm tra mật khẩu
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                    throw new Exception("INVALID PASSWORD");

                // Kiểm tra vai trò: chỉ cho phép student và instructor đăng nhập trên app
                if (user.Role != "student" )
                {
                    throw new Exception("STUDENT ALLOW ON APP");
                }

                // Đặt trạng thái người dùng là trực tuyến
                user.IsOnline = true;
                this.context.Users.Update(user);
                await this.context.SaveChangesAsync();

                // Tạo và trả về token
                var token = CreateToken(user);
                return token;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }


        public async Task<string> LoginWeb(LoginDTO request)
        {
            try
            {
                // Truy xuất người dùng dựa trên tên đăng nhập
                var user = await this.context.Users
                    .Where(x => x.Username.Equals(request.Username))
                    .FirstOrDefaultAsync();

                if (user == null)
                    throw new Exception("USER IS NOT FOUND");

                // Kiểm tra mật khẩu
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                    throw new Exception("INVALID PASSWORD");

                // Kiểm tra vai trò: chỉ cho phép admin và instructor đăng nhập
                if (user.Role != "admin" && user.Role != "instructor")
                {
                    throw new Exception("ADMIN OR INSTRUCTOR ALLOW ON WEB");
                }

                // Đặt trạng thái người dùng là trực tuyến
                user.IsOnline = true;
                this.context.Users.Update(user);
                await this.context.SaveChangesAsync();

                // Tạo và trả về token
                var token = CreateToken(user);
                return token;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }


        public async Task<User> Registration(RegisterDTO request)
        {
            try
            {
                var r = new User();
                if (request != null)
                {
                    foreach (var x in this.context.Users)
                    {
                        if (request.Username.Equals(x.Username))
                        {
                            throw new Exception("UserName has been existted!");
                        }
                    }
                    r.Id = "S" + Guid.NewGuid().ToString().Substring(0, 5);
                    r.Username = request.Username;
                    r.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                    r.Role = "student";
                    r.LastLogin= DateTime.Now;
                    r.IsOnline = true;
                    await this.context.Users.AddAsync(r);
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

        public async Task<User> CreateInstructor(CreateUserInstructorDTO request)
        {
            using var transaction = await this.context.Database.BeginTransactionAsync(); // Start a database transaction
            try
            {
                var instructorUser = new User();
                if (request != null)
                {
                    // Validate Gender
                    var validGenders = new[] { "Male", "Female" };
                    if (!validGenders.Contains(request.Gender, StringComparer.OrdinalIgnoreCase))
                    {
                        throw new Exception("Gender must be either 'Male' or 'Female'.");
                    }

                    // Check if username or email already exists
                    foreach (var x in this.context.Users)
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
                    instructorUser.Id = "I" + Guid.NewGuid().ToString().Substring(0, 5);
                    instructorUser.Username = request.Username;
                    instructorUser.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                    instructorUser.Email = request.Email;
                    instructorUser.Fullname = request.Fullname;
                    instructorUser.Education = request.Education;
                    instructorUser.Phonenumber = request.PhoneNumber;
                    instructorUser.Gender = request.Gender;
                    instructorUser.Role = "instructor";
                    instructorUser.LastLogin = DateTime.Now;
                    instructorUser.IsOnline = false;

                    // Add the instructor to the User table
                    await this.context.Users.AddAsync(instructorUser);
                    await this.context.SaveChangesAsync();

                    // Now insert instructor-specific data into the Instructor table
                    var instructor = new Instructor
                    {
                        UserId = instructorUser.Id, // Link to User.Id
                        Fullname = instructorUser.Fullname,
                        Email = instructorUser.Email,
                        Phonenumber = instructorUser.Phonenumber,
                        Status = true // Assuming new instructor is active by default, change if needed
                    };

                    // Add instructor-specific details to Instructor table
                    await this.context.Instructors.AddAsync(instructor);
                    await this.context.SaveChangesAsync();

                    await transaction.CommitAsync(); // Commit transaction

                    return instructorUser;
                }
                return null;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Rollback in case of an error
                var errorMessage = $"{ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}";
                throw new Exception(errorMessage);
            }
        }

        public async Task<User> UpdateStudent(string id, UpdateUserStudentDTO user)
        {
            try
            {
                var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
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
                    var emailExists = await context.Users.AnyAsync(x => x.Email == user.Email && x.Id != id);
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
                context.Users.Update(existingUser);
                await context.SaveChangesAsync();

                return existingUser;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<List<User>> SearchByName(string name)
        {
            try
            {
                var list = await this.context.Users.Where(x => x.Username.Contains(name)).ToListAsync();
                if (list != null) return list;
                throw new Exception("Not Found");
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<User> getUserByID(SearchUserIdDTO id)
        {
            try
            {
                var search = await this.context.Users.Where(x => x.Id.Equals(id.userID))
                                                                .FirstOrDefaultAsync();
                return search;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<User> UpdateStatusIsOnline(string userID)
        {
            try
            {
                // Find the user by userID
                var user = await this.context.Users.Where(a => a.Id == userID).FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                // Toggle the IsOnline status
                user.IsOnline = !user.IsOnline;

                // Update the user's record
                this.context.Users.Update(user);
                await this.context.SaveChangesAsync();

                return user;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<List<User>> GetStudentsByPrefix(string userIdPrefix)
        {
            // Ensure the prefix is provided
            if (string.IsNullOrEmpty(userIdPrefix))
            {
                throw new ArgumentException("User ID prefix cannot be null or empty.");
            }

            // Retrieve all students whose IDs start with the specified prefix
            var students = await this.context.Users
                .Where(u => u.Id.StartsWith(userIdPrefix) && u.Role == "student") // Use '==' for comparison
                .ToListAsync();

            return students;
        }

        public async Task<bool> Delete(RemoveUDTO id)
        {
            try
            {
                if (id != null)
                {
                    var obj = await this.context.Users.Where(x => x.Id.Equals(id.UserID)).FirstOrDefaultAsync();
                    this.context.Users.Remove(obj);
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
