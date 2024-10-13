using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.Models;
using SWD.SAPelearning.Repository.DTO.UserDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SWD.SAPelearning.Repository.DTO;
using System.Linq;

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


        public async Task<List<User>> GetAllUsers(GetAllDTO request)
        {
            try
            {
                var query = this.context.Users.AsQueryable();

                // Filtering
                if (!string.IsNullOrEmpty(request.FilterOn) && !string.IsNullOrEmpty(request.FilterQuery))
                {
                    switch (request.FilterOn.ToLower())
                    {
                        case "username":
                            query = query.Where(u => u.Username.Contains(request.FilterQuery));
                            break;
                        case "email":
                            query = query.Where(u => u.Email.Contains(request.FilterQuery));
                            break;
                        case "fullname":
                            query = query.Where(u => u.Fullname.Contains(request.FilterQuery));
                            break;
                        case "education":
                            query = query.Where(u => u.Education.Contains(request.FilterQuery));
                            break;
                        case "phonenumber":
                            query = query.Where(u => u.Phonenumber.Contains(request.FilterQuery));
                            break;
                        case "gender":
                            query = query.Where(u => u.Gender.Contains(request.FilterQuery));
                            break;
                        case "registrationdate":
                            if (DateTime.TryParse(request.FilterQuery, out var regDate))
                            {
                                query = query.Where(u => u.RegistrationDate.HasValue && u.RegistrationDate.Value.Date == regDate.Date);
                            }
                            break;
                        case "role":
                            query = query.Where(u => u.Role.Equals(request.FilterQuery, StringComparison.OrdinalIgnoreCase));
                            break;
                        case "lastlogin":
                            if (DateTime.TryParse(request.FilterQuery, out var lastLogin))
                            {
                                query = query.Where(u => u.LastLogin.HasValue && u.LastLogin.Value.Date == lastLogin.Date);
                            }
                            break;
                        case "isonline":
                            if (bool.TryParse(request.FilterQuery, out var isOnline))
                            {
                                query = query.Where(u => u.IsOnline == isOnline);
                            }
                            break;
                        default:
                            break;
                    }
                }

                // Sorting
                if (!string.IsNullOrEmpty(request.SortBy))
                {
                    if (request.IsAscending == true)
                    {
                        query = request.SortBy.ToLower() switch
                        {
                            "username" => query.OrderBy(u => u.Username),
                            "email" => query.OrderBy(u => u.Email),
                            "fullname" => query.OrderBy(u => u.Fullname),
                            "education" => query.OrderBy(u => u.Education),
                            "phonenumber" => query.OrderBy(u => u.Phonenumber),
                            "gender" => query.OrderBy(u => u.Gender),
                            "registrationdate" => query.OrderBy(u => u.RegistrationDate),
                            "role" => query.OrderBy(u => u.Role),
                            "lastlogin" => query.OrderBy(u => u.LastLogin),
                            "isonline" => query.OrderBy(u => u.IsOnline),
                            _ => query.OrderBy(u => u.Username) // Default sort
                        };
                    }
                    else if (request.IsAscending == false)
                    {
                        query = request.SortBy.ToLower() switch
                        {
                            "username" => query.OrderByDescending(u => u.Username),
                            "email" => query.OrderByDescending(u => u.Email),
                            "fullname" => query.OrderByDescending(u => u.Fullname),
                            "education" => query.OrderByDescending(u => u.Education),
                            "phonenumber" => query.OrderByDescending(u => u.Phonenumber),
                            "gender" => query.OrderByDescending(u => u.Gender),
                            "registrationdate" => query.OrderByDescending(u => u.RegistrationDate),
                            "role" => query.OrderByDescending(u => u.Role),
                            "lastlogin" => query.OrderByDescending(u => u.LastLogin),
                            "isonline" => query.OrderByDescending(u => u.IsOnline),
                            _ => query.OrderByDescending(u => u.Username) // Default sort
                        };
                    }
                    // If IsAscending is null, no sorting will be applied.
                }

                // Paging
                int pageNumber = request.PageNumber ?? 1; // Default to 1 if null
                int pageSize = request.PageSize ?? 10; // Default to 10 if null
                var totalRecords = await query.CountAsync();
                var users = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                return users;
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
                // Retrieve the user based on the username or email
                var user = await this.context.Users
                    .Where(x => x.Username.Equals(request.Username) || x.Email.Equals(request.Username))
                    .FirstOrDefaultAsync();

                if (user == null)
                    throw new Exception("USER IS NOT FOUND");

                // Check if the password is correct
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                    throw new Exception("INVALID PASSWORD");

                // Check role: only allow student to log in on the app
                if (user.Role != "student")
                {
                    throw new Exception("STUDENT ALLOW ON APP");
                }

                // Update LastLogin date/time
                user.LastLogin = DateTime.Now; // Or DateTime.UtcNow if you want UTC time

                // Set user status to online
                user.IsOnline = true;
                this.context.Users.Update(user);
                await this.context.SaveChangesAsync();

                // Create and return token (or any other mechanism, since JWT is not being used)
                var token = CreateToken(user); // Update this as per your token generation
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
                // Retrieve the user based on the username or email
                var user = await this.context.Users
                    .Where(x => x.Username.Equals(request.Username) || x.Email.Equals(request.Username))
                    .FirstOrDefaultAsync();

                if (user == null)
                    throw new Exception("USER IS NOT FOUND");

                // Check if the password is correct
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                    throw new Exception("INVALID PASSWORD");

                // Check role: only allow admin and instructor to log in on the web
                if (user.Role != "admin" && user.Role != "instructor")
                {
                    throw new Exception("ADMIN OR INSTRUCTOR ALLOW ON WEB");
                }

                // Set user status to online
                user.IsOnline = true;

                // Update LastLogin date/time
                user.LastLogin = DateTime.Now; // Or DateTime.UtcNow if you want UTC time

                // Update the user information
                this.context.Users.Update(user);
                await this.context.SaveChangesAsync();

                // Create and return token (or any other mechanism, since JWT is not being used)
                var token = CreateToken(user); // Update this as per your token generation
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
                    r.RegistrationDate = DateTime.Now;
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
                    instructorUser.RegistrationDate = DateTime.Now;
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

        public async Task<User> UpdateIsOnlineLogout(string userID)
        {
            try
            {
                // Find the user by userID
                var user = await this.context.Users.Where(a => a.Id == userID).FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                // Set IsOnline status to false for logout
                user.IsOnline = false;

                // Update the user's record
                this.context.Users.Update(user);
                await this.context.SaveChangesAsync();

                return user;
            }
            catch (Exception)
            {
                throw; // Rethrow the exception for further handling
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
                    // Find the user by UserID
                    var user = await this.context.Users
                        .Where(x => x.Id.Equals(id.UserID))
                        .FirstOrDefaultAsync();

                    if (user == null)
                    {
                        throw new Exception("User not found");
                    }

                    // Find the instructor associated with the user
                    var instructor = await this.context.Instructors
                        .Where(i => i.UserId.Equals(user.Id))
                        .FirstOrDefaultAsync();

                    // If an instructor exists, remove it
                    if (instructor != null)
                    {
                        this.context.Instructors.Remove(instructor);
                    }

                    // Remove the user
                    this.context.Users.Remove(user);
                    await this.context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Capture the error message and include inner exception if available
                var errorMessage = $"{ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $" Inner Exception: {ex.InnerException.Message}";
                }

                throw new Exception(errorMessage);
            }
        }

    }
}
