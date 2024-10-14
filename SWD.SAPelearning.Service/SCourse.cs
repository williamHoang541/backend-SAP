using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWD.SAPelearning.Repository;
using SWD.SAPelearning.Repository.DTO;

using SWD.SAPelearning.Repository.DTO.CourseDTO;
using SWD.SAPelearning.Repository.Models;
using SWD.SAPelearning.Service;

namespace SAPelearning_bakend.Repositories.Services
{
    public class SCourse:ICourse
    {
        private readonly IConfiguration _configuration;

        private readonly SAPelearningdeployContext context;

        public SCourse(SAPelearningdeployContext Context, IConfiguration configuration)
        {
            context = Context;
            _configuration = configuration;
        }


        public async Task<List<CourseDTO>> GetAllCourseAsync(GetAllDTO getAllDTO)
        {
            IQueryable<Course> query = context.Courses
                .Include(c => c.Instructor)
                .Include(c => c.Certificate)
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(getAllDTO.FilterOn) && !string.IsNullOrWhiteSpace(getAllDTO.FilterQuery))
            {
                switch (getAllDTO.FilterOn.ToLower())
                {
                    case "instructorid":
                        if (int.TryParse(getAllDTO.FilterQuery, out int instructorId))
                        {
                            query = query.Where(c => c.InstructorId == instructorId);
                        }
                        break;
                    case "certificateid":
                        if (int.TryParse(getAllDTO.FilterQuery, out int certificateId))
                        {
                            query = query.Where(c => c.CertificateId == certificateId);
                        }
                        break;
                    case "coursename":
                        query = query.Where(c => c.CourseName.Contains(getAllDTO.FilterQuery));
                        break;
                    case "starttime":
                        if (DateTime.TryParse(getAllDTO.FilterQuery, out DateTime startTime))
                        {
                            query = query.Where(c => c.StartTime >= startTime);
                        }
                        break;
                    case "endtime":
                        if (DateTime.TryParse(getAllDTO.FilterQuery, out DateTime endTime))
                        {
                            query = query.Where(c => c.EndTime <= endTime);
                        }
                        break;
                    case "mode":
                        query = query.Where(c => c.Mode.Equals(getAllDTO.FilterQuery, StringComparison.OrdinalIgnoreCase));
                        break;
                    case "price":
                        if (double.TryParse(getAllDTO.FilterQuery, out double price))
                        {
                            query = query.Where(c => c.Price == price);
                        }
                        break;
                    case "totalstudent":
                        if (int.TryParse(getAllDTO.FilterQuery, out int totalStudent))
                        {
                            query = query.Where(c => c.TotalStudent == totalStudent);
                        }
                        break;
                    case "enrollmentdate":
                        if (DateTime.TryParse(getAllDTO.FilterQuery, out DateTime enrollmentDate))
                        {
                            query = query.Where(c => c.EnrollmentDate == enrollmentDate);
                        }
                        break;
                    case "instructorname":
                        query = query.Where(c => c.Instructor.Fullname.Contains(getAllDTO.FilterQuery));
                        break;
                    case "certificatename":
                        query = query.Where(c => c.Certificate.CertificateName.Contains(getAllDTO.FilterQuery));
                        break;
                    case "location":
                        query = query.Where(c => c.Location.Contains(getAllDTO.FilterQuery));
                        break;
                    case "status":
                        if (bool.TryParse(getAllDTO.FilterQuery, out bool status))
                        {
                            query = query.Where(c => c.Status == status);
                        }
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(getAllDTO.SortBy))
            {
                bool isAscending = getAllDTO.IsAscending ?? true;

                switch (getAllDTO.SortBy.ToLower())
                {
                    case "instructorid":
                        query = isAscending
                            ? query.OrderBy(c => c.InstructorId)
                            : query.OrderByDescending(c => c.InstructorId);
                        break;
                    case "certificateid":
                        query = isAscending
                            ? query.OrderBy(c => c.CertificateId)
                            : query.OrderByDescending(c => c.CertificateId);
                        break;
                    case "coursename":
                        query = isAscending
                            ? query.OrderBy(c => c.CourseName)
                            : query.OrderByDescending(c => c.CourseName);
                        break;
                    case "starttime":
                        query = isAscending
                            ? query.OrderBy(c => c.StartTime)
                            : query.OrderByDescending(c => c.StartTime);
                        break;
                    case "endtime":
                        query = isAscending
                            ? query.OrderBy(c => c.EndTime)
                            : query.OrderByDescending(c => c.EndTime);
                        break;
                    case "mode":
                        query = isAscending
                            ? query.OrderBy(c => c.Mode)
                            : query.OrderByDescending(c => c.Mode);
                        break;
                    case "price":
                        query = isAscending
                            ? query.OrderBy(c => c.Price)
                            : query.OrderByDescending(c => c.Price);
                        break;
                    case "totalstudent":
                        query = isAscending
                            ? query.OrderBy(c => c.TotalStudent)
                            : query.OrderByDescending(c => c.TotalStudent);
                        break;
                    case "enrollmentdate":
                        query = isAscending
                            ? query.OrderBy(c => c.EnrollmentDate)
                            : query.OrderByDescending(c => c.EnrollmentDate);
                        break;
                    case "location":
                        query = isAscending
                            ? query.OrderBy(c => c.Location)
                            : query.OrderByDescending(c => c.Location);
                        break;
                    case "status":
                        query = isAscending
                            ? query.OrderBy(c => c.Status)
                            : query.OrderByDescending(c => c.Status);
                        break;
                    default:
                        // Default to sorting by MaterialName if no valid SortBy is provided
                        query = isAscending
                            ? query.OrderBy(c => c.CourseName) // Change to a default property if desired
                            : query.OrderByDescending(c => c.CourseName);
                        break;
                }
            }

                // Pagination
                int pageNumber = getAllDTO.PageNumber ?? 1;
            int pageSize = getAllDTO.PageSize ?? 10;
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            // Fetch and return courses
            var courses = await query
                .Select(c => new CourseDTO
                {
                    Id = c.Id,
                    InstructorId = c.InstructorId,
                    InstructorName = c.Instructor.Fullname, // Assuming Instructor has Fullname property
                    CertificateId = c.CertificateId,
                    CertificateName = c.Certificate.CertificateName, // Assuming Certificate has CertificateName property
                    CourseName = c.CourseName,
                    StartTime = c.StartTime,
                    EndTime = c.EndTime,
                    Mode = c.Mode,
                    Price = c.Price,
                    TotalStudent = c.TotalStudent,
                    EnrollmentDate = c.EnrollmentDate,
                    Location = c.Location,
                    Status = c.Status
                })
                .ToListAsync();

            return courses;
        }


        public async Task<CourseDTO> CreateCourse(CourseCreateDTO request)
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
                    
                    EnrollmentDate = request.EnrollmentDate,
                    Location = request.Location,
                    Status = request.Status
                };

                // Add the new course to the database
                context.Courses.Add(newCourse);
                await context.SaveChangesAsync(); // This will set newCourse.Id

                // Fetch the instructor details
                return await GetCourseById(newCourse.Id);
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
                // Fetch course by ID and include Instructor and Certificate details
                var course = await context.Courses
                    .Include(c => c.Instructor) // Assuming a relationship with Instructor
                    .Include(c => c.Certificate) // Assuming a relationship with Certificate
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (course == null)
                {
                    throw new Exception($"Course with ID {id} not found.");
                }

                // Map course data to DTO
                return new CourseDTO
                {
                    Id = course.Id,
                    InstructorId = course.InstructorId,
                    InstructorName = course.Instructor?.Fullname, // Assuming Instructor has a Fullname property
                    CertificateId = course.CertificateId,
                    CertificateName = course.Certificate?.CertificateName, // Assuming Certificate has a CertificateName property
                    CourseName = course.CourseName,
                    StartTime = course.StartTime,
                    EndTime = course.EndTime,
                    Mode = course.Mode,
                    Price = course.Price,
                    TotalStudent = course.TotalStudent,
                    EnrollmentDate = course.EnrollmentDate,
                    Location = course.Location,
                    Status = course.Status
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

        public async Task<CourseDTO> UpdateCourse(int id, CourseCreateDTO request)
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

                // Check for duplicate course name (excluding the current one)
                var duplicateCourse = await context.Courses
                    .Where(c => c.CourseName.ToLower() == request.CourseName.ToLower() && c.Id != id)
                    .FirstOrDefaultAsync();

                if (duplicateCourse != null)
                {
                    throw new Exception($"A course with the name '{request.CourseName}' already exists.");
                }

                // Validate Mode
                if (string.IsNullOrWhiteSpace(request.Mode) ||
                    !request.Mode.Equals("Online", StringComparison.OrdinalIgnoreCase) &&
                    !request.Mode.Equals("Offline", StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException("Mode must be either 'Online' or 'Offline'.");
                }

                // Validate EnrollmentDate
                if (request.EnrollmentDate >= request.StartTime)
                {
                    throw new ArgumentException("Enrollment date must be before the start time of the course.");
                }

                // Validate EndTime
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
                existingCourse.EnrollmentDate = request.EnrollmentDate;
                existingCourse.Location = request.Location;
                existingCourse.Status = request.Status;

                // Calculate the total number of enrolled students
                existingCourse.TotalStudent = await context.Enrollments.CountAsync(e => e.CourseId == existingCourse.Id);

                // Save changes to the database
                await context.SaveChangesAsync();

                // Fetch Instructor and Certificate details to include in the DTO
                var instructor = await context.Instructors.FindAsync(request.InstructorId);
                var certificate = await context.Certificates.FindAsync(request.CertificateId);

                // Return the updated course as a DTO
                return new CourseDTO
                {Id = existingCourse.Id,
                    InstructorId = existingCourse.InstructorId,
                    InstructorName = instructor?.Fullname ?? "Unknown Instructor",
                    CertificateId = existingCourse.CertificateId,
                    CertificateName = certificate?.CertificateName, // Assuming Certificate has a CertificateName property
                    CourseName = existingCourse.CourseName,
                    StartTime = existingCourse.StartTime,
                    EndTime = existingCourse.EndTime,
                    Mode = existingCourse.Mode,
                    Price = existingCourse.Price,
                    TotalStudent = existingCourse.TotalStudent, // This is now set automatically based on enrollment
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
