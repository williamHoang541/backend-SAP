using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO;
using SWD.SAPelearning.Repository.Models;

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
        public async Task<Course> CreateCourse(CourseDTO request)
        {
            try
            {
                // Validate input
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request), "CourseDTO cannot be null.");
                }

                // Check if a course with the same name already exists (case-insensitive)
                var existingCourse = await context.Courses
                    .FirstOrDefaultAsync(c => c.CourseName.ToLower() == request.CourseName.ToLower());

                if (existingCourse != null)
                {
                    throw new Exception($"A course with the name '{request.CourseName}' already exists.");
                }

                // Map DTO to Entity
                var course = new Course
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

                // Validate and set Instructor
                if (request.InstructorId != null)
                {
                    var instructor = await context.Instructors.FindAsync(request.InstructorId);
                    if (instructor == null)
                    {
                        throw new Exception($"Instructor with ID {request.InstructorId} does not exist.");
                    }

                    // If the instructor is inactive, set it as active
                    if (instructor.Status == false)
                    {
                        instructor.Status = true;
                    }

                    course.InstructorId = request.InstructorId;
                }

                // Validate and set Certificate
                if (request.CertificateId != null)
                {
                    var certificate = await context.Certificates.FindAsync(request.CertificateId);
                    if (certificate == null)
                    {
                        throw new Exception($"Certificate with ID {request.CertificateId} does not exist.");
                    }

                    // If the certificate status is False, set it to True
                    if (certificate.Status == false)
                    {
                        certificate.Status = true;
                    }

                    course.CertificateId = request.CertificateId;
                }

                // Add new course to the context
                await context.Courses.AddAsync(course);
                await context.SaveChangesAsync();

                // Return the newly created course
                return course;
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database update errors
                throw new Exception("There was an error saving the Course to the database.", dbEx);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                var errorMessage = $"{ex.Message} {(ex.InnerException != null ? ex.InnerException.Message : string.Empty)}";
                throw new Exception($"An error occurred while creating the Course: {errorMessage}");
            }
        }

        public async Task<Course> GetCourseById(int id)
        {
            var course = await context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Certificate)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                throw new Exception($"Course with ID {id} not found.");
            }

            return course;
        }
    }
}
