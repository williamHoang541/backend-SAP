using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.Models;
using SWD.SAPelearning.Service;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SCourse:ICourse
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningContext context;

        public SCourse(SAPelearningContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }


        public async Task<List<Course>> GetAllCourse()
        {
            try
            {
                var a = await this.context.Courses.ToListAsync();
                return a;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }

        }
        public async Task<CourseDTO> CreateCourse(CourseDTO request)
        {
            try
            {
                // Validate the request
                if (request == null)
                    throw new ArgumentNullException(nameof(request), "CourseDTO cannot be null.");

                // Check if a course with the same name already exists
                var existingCourse = await context.Courses
                    .FirstOrDefaultAsync(c => c.CourseName.ToLower() == request.CourseName.ToLower());

                if (existingCourse != null)
                    throw new Exception($"A course with the name '{request.CourseName}' already exists.");

                // Validate Mode
                if (request.Mode?.ToLower() != "online" && request.Mode?.ToLower() != "offline")
                    throw new ArgumentException("Mode must be either 'Online' or 'Offline'.");

                // Validate TotalStudent limits
                if (request.Mode.ToLower() == "online" && request.TotalStudent > 80)
                    throw new ArgumentException("Online courses cannot have more than 80 students.");
                if (request.Mode.ToLower() == "offline" && request.TotalStudent > 40)
                    throw new ArgumentException("Offline courses cannot have more than 40 students.");

                if (!request.StartTime.HasValue)
                    throw new ArgumentException("Start time is required.");

                if (!request.EndTime.HasValue)
                    throw new ArgumentException("End time is required.");

                if (!request.EnrollmentDate.HasValue)
                    throw new ArgumentException("Enrollment date is required.");

                // Validate EnrollmentDate
                if (request.EnrollmentDate >= request.StartTime.Value)
                    throw new ArgumentException("Enrollment date must be before the start time of the course.");

                // Check if endTime is at least 3 months after startTime
                DateTime minimumEndTime = request.StartTime.Value.AddMonths(3);
                if (request.EndTime < minimumEndTime)
                    throw new ArgumentException("End time must be at least 3 months after the start time.");

                // Map DTO to the Course entity
                var newCourse = new Course
                {
                    InstructorId = request.InstructorId,
                    CertificateId = request.CertificateId,
                    CourseName = request.CourseName,
                    StartTime = request.StartTime,
                    EndTime = request.EndTime,
                    Mode = request.Mode,
                    Price = request.Price,
                    TotalStudent = request.TotalStudent,
                    EnrollmentDate = request.EnrollmentDate,
                    Location = request.Location,
                    Status = request.Status
                };

                // Add the new course to the database
                context.Courses.Add(newCourse);
                await context.SaveChangesAsync(); // This will set newCourse.Id

                // Fetch the instructor details
                var instructor = await context.Instructors.FindAsync(newCourse.InstructorId);

                // Return the created course as a DTO
                return new CourseDTO
                {
                    CourseId = newCourse.Id,
                    InstructorId = newCourse.InstructorId,
                    InstructorName = instructor?.Fullname ?? "Unknown Instructor", // Fetch the instructor name
                    CertificateId = newCourse.CertificateId,
                    CourseName = newCourse.CourseName,
                    StartTime = newCourse.StartTime,
                    EndTime = newCourse.EndTime,
                    Mode = newCourse.Mode,
                    Price = newCourse.Price,
                    TotalStudent = newCourse.TotalStudent,
                    EnrollmentDate = newCourse.EnrollmentDate,
                    Location = newCourse.Location,
                    Status = newCourse.Status
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating the course: {ex.Message}");
            }
        }







        public async Task<CourseDTO> GetCourseById(int id)
        {
            try
            {
                // Fetch course by ID and include Instructor and Certificate
                var course = await context.Courses
                    .Include(c => c.Instructor)
                    .Include(c => c.Certificate)
                    .FirstOrDefaultAsync(c => c.Id == id); // Using Id instead of CourseId

                if (course == null)
                {
                    throw new Exception($"Course with ID {id} not found.");
                }

                // Map course data to DTO and handle possible null values
                return new CourseDTO
                {
                    InstructorId = course.InstructorId,
                    InstructorName = course.Instructor?.Fullname ?? "Unknown Instructor", // Null-safe access to Instructor's FullName
                    CertificateId = course.CertificateId,
                    CourseName = course.CourseName,
                    StartTime = course.StartTime,
                    EndTime = course.EndTime,
                    Mode = course.Mode,
                    Price = course.Price,
                    TotalStudent = course.TotalStudent,
                    EnrollmentDate = course.EnrollmentDate,
                    Location = course.Location,
                    Status = course.Status,
                     CourseId = course.Id
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the course: {ex.Message}");
            }
        }

        public async Task<bool> DeleteCourse(int id)
        {
            try
            {
                // Find the course by ID
                var course = await context.Courses
                    .Include(c => c.CourseMaterials) // Include related CourseMaterials for deletion
                    .Include(c => c.CourseSessions) // Include related CourseSessions to update status
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (course == null)
                {
                    throw new Exception($"Course with ID {id} not found."); // Course not found
                }

                // Update CourseSession status to false
                foreach (var session in course.CourseSessions)
                {
                    session.Status = false; // Set status to false
                }

                // Remove all related CourseMaterials
                context.CourseMaterials.RemoveRange(course.CourseMaterials);

                // Remove the course itself
                context.Courses.Remove(course);

                // Save changes to the database
                await context.SaveChangesAsync();

                return true; // Return true if deletion was successful
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the course: {ex.Message}"); // Handle unexpected errors
            }
        }

        public async Task<CourseDTO> UpdateCourse(int id, CourseDTO request)
        {
            try
            {
                // Validate the request
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "CourseDTO cannot be null.");
                }

                // Check if the course exists
                var existingCourse = await context.Courses.FindAsync(id);
                if (existingCourse == null)
                {
                    throw new Exception($"Course with ID {id} not found.");
                }

                // Check if a course with the same name already exists (excluding the current one)
                var duplicateCourse = await context.Courses
                    .Where(c => c.CourseName.ToLower() == request.CourseName.ToLower() && c.Id != id)
                    .FirstOrDefaultAsync();

                if (duplicateCourse != null)
                {
                    throw new Exception($"A course with the name '{request.CourseName}' already exists.");
                }

                // Check valid mode
                if (string.IsNullOrWhiteSpace(request.Mode) ||
                    (request.Mode.ToLower() != "online" && request.Mode.ToLower() != "offline"))
                {
                    throw new ArgumentException("Mode must be either 'Online' or 'Offline'.");
                }

                // Validate TotalStudent limit based on Mode
                if (request.Mode.ToLower() == "online" && request.TotalStudent > 80)
                {
                    throw new ArgumentException("Online courses cannot have more than 80 students.");
                }
                if (request.Mode.ToLower() == "offline" && request.TotalStudent > 40)
                {
                    throw new ArgumentException("Offline courses cannot have more than 40 students.");
                }

                // Validate EnrollmentDate
                if (request.EnrollmentDate >= request.StartTime)
                {
                    throw new ArgumentException("Enrollment date must be before the start time of the course.");
                }

                // Check if endTime is at least 3 months after startTime
                if (request.StartTime.HasValue && request.EndTime.HasValue)
                {
                    DateTime minimumEndTime = request.StartTime.Value.AddMonths(3);
                    if (request.EndTime < minimumEndTime)
                    {
                        throw new ArgumentException("End time must be at least 3 months after the start time.");
                    }
                }

                // Update properties
                existingCourse.InstructorId = request.InstructorId;
                existingCourse.CertificateId = request.CertificateId;
                existingCourse.CourseName = request.CourseName;
                existingCourse.StartTime = request.StartTime;
                existingCourse.EndTime = request.EndTime;
                existingCourse.Mode = request.Mode;
                existingCourse.Price = request.Price;
                existingCourse.TotalStudent = request.TotalStudent;
                existingCourse.EnrollmentDate = request.EnrollmentDate;
                existingCourse.Location = request.Location;
                existingCourse.Status = request.Status;

                // Save changes
                await context.SaveChangesAsync();

                // Fetch Instructor and Certificate details to include in the DTO
                var instructor = await context.Instructors.FindAsync(request.InstructorId);
                var certificate = await context.Certificates.FindAsync(request.CertificateId);

                // Return the updated course as a DTO
                return new CourseDTO
                {
                    CourseId = existingCourse.Id,
                    InstructorId = existingCourse.InstructorId,
                    InstructorName = instructor?.Fullname ?? "Unknown Instructor",
                    CertificateId = existingCourse.CertificateId,
                    CourseName = existingCourse.CourseName,
                    StartTime = existingCourse.StartTime,
                    EndTime = existingCourse.EndTime,
                    Mode = existingCourse.Mode,
                    Price = existingCourse.Price,
                    TotalStudent = existingCourse.TotalStudent,
                    EnrollmentDate = existingCourse.EnrollmentDate,
                    Location = existingCourse.Location,
                    Status = existingCourse.Status
                };
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("There was an error updating the course in the database.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the course: {ex.Message}");
            }
        }




    }
}
